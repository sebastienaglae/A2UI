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

namespace A2UI.Unity.Core
{
    /// <summary>
    /// Represents an A2UI component node.
    /// </summary>
    public class ComponentNode
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public Dictionary<string, object> Properties { get; set; }
        public List<string> Children { get; set; }
        public float? Weight { get; set; }
        public List<string> ClassName { get; set; }
        
        public ComponentNode()
        {
            Properties = new Dictionary<string, object>();
            Children = new List<string>();
            ClassName = new List<string>();
        }
        
        /// <summary>
        /// Get a property value by key.
        /// </summary>
        public T GetProperty<T>(string key, T defaultValue = default)
        {
            if (Properties == null || !Properties.ContainsKey(key))
                return defaultValue;
            
            try
            {
                var value = Properties[key];
                if (value is T typedValue)
                    return typedValue;
                    
                // Try to convert
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }
        
        /// <summary>
        /// Check if a property exists.
        /// </summary>
        public bool HasProperty(string key)
        {
            return Properties != null && Properties.ContainsKey(key);
        }
    }
    
    /// <summary>
    /// Represents the data model for a surface.
    /// </summary>
    public class DataModel
    {
        private readonly Dictionary<string, object> data;
        
        public DataModel()
        {
            data = new Dictionary<string, object>();
        }
        
        public DataModel(Dictionary<string, object> data)
        {
            this.data = data ?? new Dictionary<string, object>();
        }
        
        /// <summary>
        /// Get data by JSON pointer path (e.g., "/user/name").
        /// </summary>
        public object GetValue(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;
            
            // Remove leading slash if present
            if (path.StartsWith("/"))
                path = path.Substring(1);
            
            if (string.IsNullOrEmpty(path))
                return data;
            
            var parts = path.Split('/');
            object current = data;
            
            foreach (var part in parts)
            {
                if (current is Dictionary<string, object> dict)
                {
                    if (dict.ContainsKey(part))
                    {
                        current = dict[part];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            
            return current;
        }
        
        /// <summary>
        /// Set data at JSON pointer path.
        /// </summary>
        public void SetValue(string path, object value)
        {
            if (string.IsNullOrEmpty(path))
                return;
            
            // Remove leading slash if present
            if (path.StartsWith("/"))
                path = path.Substring(1);
            
            var parts = path.Split('/');
            var current = data;
            
            for (int i = 0; i < parts.Length - 1; i++)
            {
                var part = parts[i];
                if (!current.ContainsKey(part) || !(current[part] is Dictionary<string, object>))
                {
                    current[part] = new Dictionary<string, object>();
                }
                current = current[part] as Dictionary<string, object>;
            }
            
            current[parts[parts.Length - 1]] = value;
        }
        
        /// <summary>
        /// Get all data.
        /// </summary>
        public Dictionary<string, object> GetAllData()
        {
            return data;
        }
    }
    
    /// <summary>
    /// Represents an A2UI error.
    /// </summary>
    public class A2UIError
    {
        public string Message { get; set; }
        public Exception Exception { get; set; }
        
        public A2UIError(string message, Exception exception = null)
        {
            Message = message;
            Exception = exception;
        }
    }
}
