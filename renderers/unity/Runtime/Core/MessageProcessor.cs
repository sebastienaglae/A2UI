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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace A2UI.Unity.Core
{
    /// <summary>
    /// Processes A2UI JSONL messages and dispatches events.
    /// </summary>
    public class MessageProcessor
    {
        private readonly ComponentFactory componentFactory;
        
        public event Action<string, List<ComponentNode>> OnSurfaceUpdate;
        public event Action<string, DataModel> OnDataModelUpdate;
        public event Action<string, string, string> OnBeginRendering;
        public event Action<string> OnDeleteSurface;
        public event Action<A2UIError> OnError;
        
        public MessageProcessor(ComponentFactory componentFactory)
        {
            this.componentFactory = componentFactory ?? throw new ArgumentNullException(nameof(componentFactory));
        }
        
        /// <summary>
        /// Process a single JSONL message.
        /// </summary>
        public void ProcessMessage(string jsonMessage)
        {
            if (string.IsNullOrWhiteSpace(jsonMessage))
                return;
            
            try
            {
                var jObject = JObject.Parse(jsonMessage);
                var messageType = jObject["messageType"]?.ToString();
                
                if (string.IsNullOrEmpty(messageType))
                {
                    OnError?.Invoke(new A2UIError("Message missing 'messageType' field"));
                    return;
                }
                
                switch (messageType)
                {
                    case "surfaceUpdate":
                        ProcessSurfaceUpdate(jObject);
                        break;
                    case "dataModelUpdate":
                        ProcessDataModelUpdate(jObject);
                        break;
                    case "beginRendering":
                        ProcessBeginRendering(jObject);
                        break;
                    case "deleteSurface":
                        ProcessDeleteSurface(jObject);
                        break;
                    default:
                        OnError?.Invoke(new A2UIError($"Unknown message type: {messageType}"));
                        break;
                }
            }
            catch (JsonException ex)
            {
                OnError?.Invoke(new A2UIError($"JSON parsing error: {ex.Message}", ex));
            }
            catch (Exception ex)
            {
                OnError?.Invoke(new A2UIError($"Error processing message: {ex.Message}", ex));
            }
        }
        
        private void ProcessSurfaceUpdate(JObject message)
        {
            var surfaceId = message["surfaceId"]?.ToString() ?? "default";
            var componentsArray = message["components"] as JArray;
            
            if (componentsArray == null)
            {
                OnError?.Invoke(new A2UIError("surfaceUpdate missing 'components' array"));
                return;
            }
            
            var components = new List<ComponentNode>();
            foreach (var componentToken in componentsArray)
            {
                try
                {
                    var component = ParseComponent(componentToken as JObject);
                    if (component != null)
                    {
                        components.Add(component);
                    }
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(new A2UIError($"Error parsing component: {ex.Message}", ex));
                }
            }
            
            OnSurfaceUpdate?.Invoke(surfaceId, components);
        }
        
        private void ProcessDataModelUpdate(JObject message)
        {
            var surfaceId = message["surfaceId"]?.ToString() ?? "default";
            var data = message["data"];
            
            if (data == null)
            {
                OnError?.Invoke(new A2UIError("dataModelUpdate missing 'data' field"));
                return;
            }
            
            var dataModel = new DataModel(data.ToObject<Dictionary<string, object>>());
            OnDataModelUpdate?.Invoke(surfaceId, dataModel);
        }
        
        private void ProcessBeginRendering(JObject message)
        {
            var surfaceId = message["surfaceId"]?.ToString() ?? "default";
            var rootComponentId = message["rootComponentId"]?.ToString();
            var catalogId = message["catalogId"]?.ToString();
            
            if (string.IsNullOrEmpty(rootComponentId))
            {
                OnError?.Invoke(new A2UIError("beginRendering missing 'rootComponentId' field"));
                return;
            }
            
            OnBeginRendering?.Invoke(surfaceId, rootComponentId, catalogId);
        }
        
        private void ProcessDeleteSurface(JObject message)
        {
            var surfaceId = message["surfaceId"]?.ToString() ?? "default";
            OnDeleteSurface?.Invoke(surfaceId);
        }
        
        private ComponentNode ParseComponent(JObject componentObj)
        {
            if (componentObj == null)
                return null;
            
            var component = new ComponentNode
            {
                Id = componentObj["id"]?.ToString(),
                Type = componentObj["type"]?.ToString(),
                Properties = new Dictionary<string, object>()
            };
            
            // Parse children
            var childrenArray = componentObj["children"] as JArray;
            if (childrenArray != null)
            {
                component.Children = new List<string>();
                foreach (var child in childrenArray)
                {
                    component.Children.Add(child.ToString());
                }
            }
            
            // Parse weight
            if (componentObj["weight"] != null)
            {
                component.Weight = componentObj["weight"].ToObject<float>();
            }
            
            // Parse properties
            var properties = componentObj["properties"] as JObject;
            if (properties != null)
            {
                component.Properties = properties.ToObject<Dictionary<string, object>>();
            }
            
            // Parse className
            var classNameArray = componentObj["className"] as JArray;
            if (classNameArray != null)
            {
                component.ClassName = new List<string>();
                foreach (var className in classNameArray)
                {
                    component.ClassName.Add(className.ToString());
                }
            }
            
            return component;
        }
    }
}
