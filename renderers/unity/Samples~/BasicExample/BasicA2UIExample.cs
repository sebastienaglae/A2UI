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

using UnityEngine;
using UnityEngine.UIElements;
using A2UI.Unity.Core;

namespace A2UI.Unity.Samples
{
    /// <summary>
    /// Basic example of using the A2UI Unity renderer.
    /// </summary>
    public class BasicA2UIExample : MonoBehaviour
    {
        [SerializeField]
        private UIDocument uiDocument;
        
        private A2UIRenderer renderer;
        
        void Start()
        {
            if (uiDocument == null)
            {
                Debug.LogError("UIDocument is not assigned!");
                return;
            }
            
            // Create the renderer
            renderer = new A2UIRenderer(uiDocument.rootVisualElement);
            
            // Subscribe to events
            renderer.OnSurfaceUpdate += OnSurfaceUpdated;
            renderer.OnUserAction += OnUserAction;
            renderer.OnError += OnError;
            
            // Example: Process a simple A2UI message
            ProcessExampleMessage();
        }
        
        void ProcessExampleMessage()
        {
            // Example A2UI messages (JSONL format)
            string[] messages = new string[]
            {
                // Surface update with components
                @"{""messageType"":""surfaceUpdate"",""surfaceId"":""main"",""components"":[{""id"":""root"",""type"":""Column"",""children"":[""title"",""description"",""button""]},{""id"":""title"",""type"":""Text"",""properties"":{""text"":{""literalString"":""Welcome to A2UI""},""usageHint"":""h1""}},{""id"":""description"",""type"":""Text"",""properties"":{""text"":{""literalString"":""This is a Unity UI Toolkit renderer for A2UI.""}}},{""id"":""button"",""type"":""Button"",""children"":[""buttonText""]},{""id"":""buttonText"",""type"":""Text"",""properties"":{""text"":{""literalString"":""Click Me""}}}]}",
                
                // Begin rendering
                @"{""messageType"":""beginRendering"",""surfaceId"":""main"",""rootComponentId"":""root""}"
            };
            
            foreach (var message in messages)
            {
                renderer.ProcessMessage(message);
            }
        }
        
        void OnSurfaceUpdated(string surfaceId, VisualElement rootElement)
        {
            Debug.Log($"Surface '{surfaceId}' updated");
        }
        
        void OnUserAction(UserActionEvent action)
        {
            Debug.Log($"User action: {action.ActionType} on component {action.ComponentId}");
            // In a real application, you would send this back to the agent via A2A protocol
        }
        
        void OnError(A2UIError error)
        {
            Debug.LogError($"A2UI Error: {error.Message}");
            if (error.Exception != null)
            {
                Debug.LogException(error.Exception);
            }
        }
        
        void OnDestroy()
        {
            if (renderer != null)
            {
                renderer.ClearAllSurfaces();
            }
        }
    }
}
