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
using A2UI.Unity.Utils;

#if NEWTONSOFT_JSON_AVAILABLE
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#endif

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
#if NEWTONSOFT_JSON_AVAILABLE
                var jObject = JObject.Parse(jsonMessage);
                var messageType = jObject["messageType"]?.ToString();
#else
                // Simplified parsing without Newtonsoft.Json
                var dict = JsonHelper.ParseObject(jsonMessage);
                var messageType = dict.ContainsKey("messageType") ? dict["messageType"]?.ToString() : null;
                var jObject = (object)dict;
#endif
                
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
#if NEWTONSOFT_JSON_AVAILABLE
            catch (JsonException ex)
            {
                OnError?.Invoke(new A2UIError($"JSON parsing error: {ex.Message}", ex));
            }
#endif
            catch (Exception ex)
            {
                OnError?.Invoke(new A2UIError($"Error processing message: {ex.Message}", ex));
            }
        }
        
        private void ProcessSurfaceUpdate(object message)
        {
#if NEWTONSOFT_JSON_AVAILABLE
            var jMsg = message as JObject;
            var surfaceId = jMsg["surfaceId"]?.ToString() ?? "default";
            var componentsArray = jMsg["components"] as JArray;
#else
            var dict = message as Dictionary<string, object>;
            var surfaceId = JsonHelper.GetString(dict, "surfaceId");
            if (string.IsNullOrEmpty(surfaceId)) surfaceId = "default";
            var componentsArray = JsonHelper.GetValue(dict, "components");
#endif
            
            if (componentsArray == null)
            {
                OnError?.Invoke(new A2UIError("surfaceUpdate missing 'components' array"));
                return;
            }
            
            var components = new List<ComponentNode>();
#if NEWTONSOFT_JSON_AVAILABLE
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
#else
            // Simplified component parsing without Newtonsoft
            if (componentsArray is List<object> list)
            {
                foreach (var item in list)
                {
                    try
                    {
                        var component = ParseComponent(item);
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
            }
#endif
            
            OnSurfaceUpdate?.Invoke(surfaceId, components);
        }
        
        private void ProcessDataModelUpdate(object message)
        {
#if NEWTONSOFT_JSON_AVAILABLE
            var jMsg = message as JObject;
            var surfaceId = jMsg["surfaceId"]?.ToString() ?? "default";
            var data = jMsg["data"];
            
            if (data == null)
            {
                OnError?.Invoke(new A2UIError("dataModelUpdate missing 'data' field"));
                return;
            }
            
            var dataModel = new DataModel(data.ToObject<Dictionary<string, object>>());
#else
            var dict = message as Dictionary<string, object>;
            var surfaceId = JsonHelper.GetString(dict, "surfaceId");
            if (string.IsNullOrEmpty(surfaceId)) surfaceId = "default";
            var data = JsonHelper.GetValue(dict, "data");
            
            if (data == null)
            {
                OnError?.Invoke(new A2UIError("dataModelUpdate missing 'data' field"));
                return;
            }
            
            var dataModel = new DataModel(data as Dictionary<string, object>);
#endif
            OnDataModelUpdate?.Invoke(surfaceId, dataModel);
        }
        
        private void ProcessBeginRendering(object message)
        {
#if NEWTONSOFT_JSON_AVAILABLE
            var jMsg = message as JObject;
            var surfaceId = jMsg["surfaceId"]?.ToString() ?? "default";
            var rootComponentId = jMsg["rootComponentId"]?.ToString();
            var catalogId = jMsg["catalogId"]?.ToString();
#else
            var dict = message as Dictionary<string, object>;
            var surfaceId = JsonHelper.GetString(dict, "surfaceId");
            if (string.IsNullOrEmpty(surfaceId)) surfaceId = "default";
            var rootComponentId = JsonHelper.GetString(dict, "rootComponentId");
            var catalogId = JsonHelper.GetString(dict, "catalogId");
#endif
            
            if (string.IsNullOrEmpty(rootComponentId))
            {
                OnError?.Invoke(new A2UIError("beginRendering missing 'rootComponentId' field"));
                return;
            }
            
            OnBeginRendering?.Invoke(surfaceId, rootComponentId, catalogId);
        }
        
        private void ProcessDeleteSurface(object message)
        {
#if NEWTONSOFT_JSON_AVAILABLE
            var jMsg = message as JObject;
            var surfaceId = jMsg["surfaceId"]?.ToString() ?? "default";
#else
            var dict = message as Dictionary<string, object>;
            var surfaceId = JsonHelper.GetString(dict, "surfaceId");
            if (string.IsNullOrEmpty(surfaceId)) surfaceId = "default";
#endif
            OnDeleteSurface?.Invoke(surfaceId);
        }
        
        private ComponentNode ParseComponent(object componentObj)
        {
            if (componentObj == null)
                return null;
            
#if NEWTONSOFT_JSON_AVAILABLE
            var jObj = componentObj as JObject;
            var component = new ComponentNode
            {
                Id = jObj["id"]?.ToString(),
                Type = jObj["type"]?.ToString(),
                Properties = new Dictionary<string, object>()
            };
            
            // Parse children
            var childrenArray = jObj["children"] as JArray;
            if (childrenArray != null)
            {
                component.Children = new List<string>();
                foreach (var child in childrenArray)
                {
                    component.Children.Add(child.ToString());
                }
            }
            
            // Parse weight
            if (jObj["weight"] != null)
            {
                component.Weight = jObj["weight"].ToObject<float>();
            }
            
            // Parse properties
            var properties = jObj["properties"] as JObject;
            if (properties != null)
            {
                component.Properties = properties.ToObject<Dictionary<string, object>>();
            }
            
            // Parse className
            var classNameArray = jObj["className"] as JArray;
#else
            var dict = componentObj as Dictionary<string, object>;
            var component = new ComponentNode
            {
                Id = JsonHelper.GetString(dict, "id"),
                Type = JsonHelper.GetString(dict, "type"),
                Properties = new Dictionary<string, object>()
            };
            
            // Parse children (simplified)
            var childrenArray = JsonHelper.GetValue(dict, "children");
            if (childrenArray is List<object> childList)
            {
                component.Children = new List<string>();
                foreach (var child in childList)
                {
                    component.Children.Add(child?.ToString());
                }
            }
            
            // Parse weight (simplified)
            var weightValue = JsonHelper.GetValue(dict, "weight");
            if (weightValue != null && float.TryParse(weightValue.ToString(), out var weight))
            {
                component.Weight = weight;
            }
            
            // Parse properties (simplified)
            var properties = JsonHelper.GetValue(dict, "properties");
            if (properties is Dictionary<string, object> propDict)
            {
                component.Properties = propDict;
            }
            
            // Parse className (simplified)
            var classNameArray = JsonHelper.GetValue(dict, "className");
#endif
            
#if NEWTONSOFT_JSON_AVAILABLE
            if (classNameArray != null && classNameArray is JArray jArray)
            {
                component.ClassName = new List<string>();
                foreach (var className in jArray)
                {
                    component.ClassName.Add(className.ToString());
                }
            }
#else
            if (classNameArray is List<object> classList)
            {
                component.ClassName = new List<string>();
                foreach (var className in classList)
                {
                    component.ClassName.Add(className?.ToString());
                }
            }
#endif
            
            return component;
        }
    }
}
