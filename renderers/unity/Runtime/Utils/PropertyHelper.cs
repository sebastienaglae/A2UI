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

using System.Collections.Generic;
using A2UI.Unity.Core;

#if NEWTONSOFT_JSON_AVAILABLE
using Newtonsoft.Json.Linq;
#endif

namespace A2UI.Unity.Utils
{
    /// <summary>
    /// Helper methods for extracting property values from components.
    /// </summary>
    public static class PropertyHelper
    {
        /// <summary>
        /// Get a string value from component properties, supporting both literal and path-based values.
        /// </summary>
        public static string GetStringValue(ComponentNode component, string propertyName, DataModel dataModel)
        {
            if (!component.HasProperty(propertyName))
                return string.Empty;
            
            var property = component.Properties[propertyName];
            
            // Handle simple string
            if (property is string strValue)
                return strValue;
            
#if NEWTONSOFT_JSON_AVAILABLE
            // Handle object with literalString or path
            if (property is JObject jObj)
            {
                if (jObj.ContainsKey("literalString"))
                {
                    return jObj["literalString"]?.ToString() ?? string.Empty;
                }
                
                if (jObj.ContainsKey("path"))
                {
                    var path = jObj["path"]?.ToString();
                    if (!string.IsNullOrEmpty(path))
                    {
                        var value = dataModel.GetValue(path);
                        return value?.ToString() ?? string.Empty;
                    }
                }
            }
#endif
                }
            }
            
            // Handle dictionary
            if (property is Dictionary<string, object> dict)
            {
                if (dict.ContainsKey("literalString"))
                {
                    return dict["literalString"]?.ToString() ?? string.Empty;
                }
                
                if (dict.ContainsKey("path"))
                {
                    var path = dict["path"]?.ToString();
                    if (!string.IsNullOrEmpty(path))
                    {
                        var value = dataModel.GetValue(path);
                        return value?.ToString() ?? string.Empty;
                    }
                }
            }
            
            return property?.ToString() ?? string.Empty;
        }
        
        /// <summary>
        /// Get a numeric value from component properties.
        /// </summary>
        public static float GetFloatValue(ComponentNode component, string propertyName, DataModel dataModel, float defaultValue = 0f)
        {
            var strValue = GetStringValue(component, propertyName, dataModel);
            if (float.TryParse(strValue, out var result))
                return result;
            return defaultValue;
        }
        
        /// <summary>
        /// Get a boolean value from component properties.
        /// </summary>
        public static bool GetBoolValue(ComponentNode component, string propertyName, DataModel dataModel, bool defaultValue = false)
        {
            var strValue = GetStringValue(component, propertyName, dataModel);
            if (bool.TryParse(strValue, out var result))
                return result;
            return defaultValue;
        }
    }
}
