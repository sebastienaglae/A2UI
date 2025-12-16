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
using System.Linq;
using UnityEngine.UIElements;

namespace A2UI.Unity.Core
{
    /// <summary>
    /// Represents a single A2UI surface (UI region).
    /// </summary>
    public class Surface
    {
        public string SurfaceId { get; private set; }
        public VisualElement RootVisualElement { get; private set; }
        
        private readonly VisualElement containerElement;
        private readonly Dictionary<string, ComponentNode> components;
        private readonly ComponentFactory componentFactory;
        private DataModel dataModel;
        private string rootComponentId;
        
        public Surface(string surfaceId, VisualElement containerElement)
        {
            this.SurfaceId = surfaceId ?? throw new ArgumentNullException(nameof(surfaceId));
            this.containerElement = containerElement ?? throw new ArgumentNullException(nameof(containerElement));
            this.components = new Dictionary<string, ComponentNode>();
            this.componentFactory = new ComponentFactory();
            this.dataModel = new DataModel();
            
            // Create a container for this surface
            this.RootVisualElement = new VisualElement();
            this.RootVisualElement.name = $"a2ui-surface-{surfaceId}";
            this.RootVisualElement.AddToClassList("a2ui-surface");
            this.containerElement.Add(this.RootVisualElement);
        }
        
        /// <summary>
        /// Update components in this surface.
        /// </summary>
        public void UpdateComponents(List<ComponentNode> newComponents)
        {
            foreach (var component in newComponents)
            {
                components[component.Id] = component;
            }
        }
        
        /// <summary>
        /// Update the data model for this surface.
        /// </summary>
        public void UpdateDataModel(DataModel newDataModel)
        {
            this.dataModel = newDataModel ?? new DataModel();
            
            // Re-render if we already have a root component
            if (!string.IsNullOrEmpty(rootComponentId))
            {
                Render();
            }
        }
        
        /// <summary>
        /// Begin rendering the surface with the specified root component.
        /// </summary>
        public void BeginRendering(string rootComponentId)
        {
            this.rootComponentId = rootComponentId;
            Render();
        }
        
        /// <summary>
        /// Clear the surface.
        /// </summary>
        public void Clear()
        {
            RootVisualElement.Clear();
            components.Clear();
            dataModel = new DataModel();
            rootComponentId = null;
            
            // Remove from container if it's still a child
            if (RootVisualElement.parent == containerElement)
            {
                containerElement.Remove(RootVisualElement);
            }
        }
        
        private void Render()
        {
            if (string.IsNullOrEmpty(rootComponentId))
                return;
                
            if (!components.TryGetValue(rootComponentId, out var rootComponent))
                return;
            
            // Clear existing content
            RootVisualElement.Clear();
            
            // Render the component tree
            var rootElement = RenderComponent(rootComponent);
            if (rootElement != null)
            {
                RootVisualElement.Add(rootElement);
            }
        }
        
        private VisualElement RenderComponent(ComponentNode component)
        {
            if (component == null)
                return null;
            
            // Create the visual element for this component
            var element = componentFactory.CreateComponent(component, dataModel);
            
            if (element == null)
                return null;
            
            // Set common properties
            element.name = component.Id;
            element.userData = component;
            
            // Render children
            if (component.Children != null && component.Children.Count > 0)
            {
                foreach (var childId in component.Children)
                {
                    if (components.TryGetValue(childId, out var childComponent))
                    {
                        var childElement = RenderComponent(childComponent);
                        if (childElement != null)
                        {
                            element.Add(childElement);
                        }
                    }
                }
            }
            
            return element;
        }
    }
}
