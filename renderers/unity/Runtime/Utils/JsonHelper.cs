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

#if NEWTONSOFT_JSON_AVAILABLE
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#else
using UnityEngine;
#endif

namespace A2UI.Unity.Utils
{
    /// <summary>
    /// JSON parsing utilities that work with or without Newtonsoft.Json.
    /// Falls back to Unity's JsonUtility when Newtonsoft.Json is not available.
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Parse a JSON string into a dictionary.
        /// </summary>
        public static Dictionary<string, object> ParseObject(string json)
        {
#if NEWTONSOFT_JSON_AVAILABLE
            var jObject = JObject.Parse(json);
            return jObject.ToObject<Dictionary<string, object>>();
#else
            // Unity's JsonUtility doesn't support Dictionary directly
            // This is a simplified fallback - in production you'd need a more robust solution
            Debug.LogWarning("A2UI: Using Unity JsonUtility fallback. For full functionality, install Newtonsoft.Json via Package Manager.");
            return new Dictionary<string, object>();
#endif
        }

        /// <summary>
        /// Get a string value from an object.
        /// </summary>
        public static string GetString(object obj, string key)
        {
#if NEWTONSOFT_JSON_AVAILABLE
            if (obj is JObject jObj && jObj.ContainsKey(key))
            {
                return jObj[key]?.ToString() ?? string.Empty;
            }
#endif
            if (obj is Dictionary<string, object> dict && dict.ContainsKey(key))
            {
                return dict[key]?.ToString() ?? string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        /// Check if an object contains a key.
        /// </summary>
        public static bool ContainsKey(object obj, string key)
        {
#if NEWTONSOFT_JSON_AVAILABLE
            if (obj is JObject jObj)
            {
                return jObj.ContainsKey(key);
            }
#endif
            if (obj is Dictionary<string, object> dict)
            {
                return dict.ContainsKey(key);
            }
            return false;
        }

        /// <summary>
        /// Get a value from an object.
        /// </summary>
        public static object GetValue(object obj, string key)
        {
#if NEWTONSOFT_JSON_AVAILABLE
            if (obj is JObject jObj && jObj.ContainsKey(key))
            {
                return jObj[key];
            }
#endif
            if (obj is Dictionary<string, object> dict && dict.ContainsKey(key))
            {
                return dict[key];
            }
            return null;
        }

        /// <summary>
        /// Serialize an object to JSON.
        /// </summary>
        public static string Serialize(object obj)
        {
#if NEWTONSOFT_JSON_AVAILABLE
            return JsonConvert.SerializeObject(obj);
#else
            return JsonUtility.ToJson(obj);
#endif
        }
    }
}
