/*
 Copyright 2025 Google LLC

 Licensed under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License.
 You may obtain a copy of the License at

      https://www.apache.org/licenses/LICENSE-2.0

 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.
 */

using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace A2UI.Unity.Core
{
    /// <summary>
    /// Main renderer class that processes A2UI messages and manages surfaces.
    /// </summary>
    public class A2UIRenderer
    {
        private readonly VisualElement rootElement;
        private readonly Dictionary<string, Surface> surfaces;
        private readonly ComponentFactory componentFactory;
        private readonly MessageProcessor messageProcessor;
        
        /// <summary>
        /// Event fired when a surface is updated.
        /// </summary>
        public event Action<string, VisualElement> OnSurfaceUpdate;
        
        /// <summary>
        /// Event fired when a user action occurs.
        /// </summary>
        public event Action<UserActionEvent> OnUserAction;
        
        /// <summary>
        /// Event fired when an error occurs.
        /// </summary>
        public event Action<A2UIError> OnError;
        
        /// <summary>
        /// Event fired when data model is updated.
        /// </summary>
        public event Action<string, string, object> OnDataModelUpdate;
        
        public A2UIRenderer(VisualElement rootElement)
        {
            this.rootElement = rootElement ?? throw new ArgumentNullException(nameof(rootElement));
            this.surfaces = new Dictionary<string, Surface>();
            this.componentFactory = new ComponentFactory();
            this.messageProcessor = new MessageProcessor(this.componentFactory);
            
            // Wire up internal events
            this.messageProcessor.OnSurfaceUpdate += HandleSurfaceUpdate;
            this.messageProcessor.OnDataModelUpdate += HandleDataModelUpdate;
            this.messageProcessor.OnBeginRendering += HandleBeginRendering;
            this.messageProcessor.OnDeleteSurface += HandleDeleteSurface;
            this.messageProcessor.OnError += HandleError;
        }
        
        /// <summary>
        /// Process a single A2UI JSONL message.
        /// </summary>
        public void ProcessMessage(string jsonMessage)
        {
            if (string.IsNullOrWhiteSpace(jsonMessage))
                return;
                
            try
            {
                messageProcessor.ProcessMessage(jsonMessage);
            }
            catch (Exception ex)
            {
                HandleError(new A2UIError($"Failed to process message: {ex.Message}", ex));
            }
        }
        
        /// <summary>
        /// Apply a custom USS stylesheet to all surfaces.
        /// </summary>
        public void ApplyStyleSheet(StyleSheet styleSheet)
        {
            if (styleSheet == null)
                throw new ArgumentNullException(nameof(styleSheet));
                
            rootElement.styleSheets.Add(styleSheet);
        }
        
        /// <summary>
        /// Register a custom component renderer.
        /// </summary>
        public void RegisterCustomComponent(string componentType, Func<ComponentNode, DataModel, VisualElement> renderer)
        {
            componentFactory.RegisterCustomRenderer(componentType, renderer);
        }
        
        /// <summary>
        /// Get a surface by ID.
        /// </summary>
        public Surface GetSurface(string surfaceId)
        {
            return surfaces.TryGetValue(surfaceId, out var surface) ? surface : null;
        }
        
        /// <summary>
        /// Clear all surfaces.
        /// </summary>
        public void ClearAllSurfaces()
        {
            foreach (var surface in surfaces.Values)
            {
                surface.Clear();
            }
            surfaces.Clear();
            rootElement.Clear();
        }
        
        private void HandleSurfaceUpdate(string surfaceId, List<ComponentNode> components)
        {
            if (!surfaces.TryGetValue(surfaceId, out var surface))
            {
                surface = new Surface(surfaceId, rootElement);
                surfaces[surfaceId] = surface;
            }
            
            surface.UpdateComponents(components);
        }
        
        private void HandleDataModelUpdate(string surfaceId, DataModel dataModel)
        {
            if (surfaces.TryGetValue(surfaceId, out var surface))
            {
                surface.UpdateDataModel(dataModel);
                OnDataModelUpdate?.Invoke(surfaceId, "", dataModel);
            }
        }
        
        private void HandleBeginRendering(string surfaceId, string rootComponentId, string catalogId)
        {
            if (surfaces.TryGetValue(surfaceId, out var surface))
            {
                surface.BeginRendering(rootComponentId);
                OnSurfaceUpdate?.Invoke(surfaceId, surface.RootVisualElement);
            }
        }
        
        private void HandleDeleteSurface(string surfaceId)
        {
            if (surfaces.TryGetValue(surfaceId, out var surface))
            {
                surface.Clear();
                surfaces.Remove(surfaceId);
            }
        }
        
        private void HandleError(A2UIError error)
        {
            OnError?.Invoke(error);
        }
    }
}
