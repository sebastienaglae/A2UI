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
using UnityEngine;
using UnityEngine.UIElements;
using A2UI.Unity.Components;

namespace A2UI.Unity.Core
{
    /// <summary>
    /// Factory for creating Unity UI elements from A2UI component definitions.
    /// </summary>
    public class ComponentFactory
    {
        private readonly Dictionary<string, Func<ComponentNode, DataModel, VisualElement>> customRenderers;
        
        public ComponentFactory()
        {
            customRenderers = new Dictionary<string, Func<ComponentNode, DataModel, VisualElement>>();
        }
        
        /// <summary>
        /// Register a custom component renderer.
        /// </summary>
        public void RegisterCustomRenderer(string componentType, Func<ComponentNode, DataModel, VisualElement> renderer)
        {
            if (string.IsNullOrEmpty(componentType))
                throw new ArgumentException("Component type cannot be null or empty", nameof(componentType));
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));
                
            customRenderers[componentType] = renderer;
        }
        
        /// <summary>
        /// Create a Unity UI element from an A2UI component.
        /// </summary>
        public VisualElement CreateComponent(ComponentNode component, DataModel dataModel)
        {
            if (component == null)
                return null;
            
            // Check for custom renderer first
            if (customRenderers.TryGetValue(component.Type, out var customRenderer))
            {
                return customRenderer(component, dataModel);
            }
            
            // Use built-in renderers
            VisualElement element = component.Type switch
            {
                "Text" => TextComponent.Create(component, dataModel),
                "Button" => ButtonComponent.Create(component, dataModel),
                "TextField" => TextFieldComponent.Create(component, dataModel),
                "Image" => ImageComponent.Create(component, dataModel),
                "Icon" => IconComponent.Create(component, dataModel),
                "Column" => ColumnComponent.Create(component, dataModel),
                "Row" => RowComponent.Create(component, dataModel),
                "Card" => CardComponent.Create(component, dataModel),
                "Checkbox" => CheckboxComponent.Create(component, dataModel),
                "Slider" => SliderComponent.Create(component, dataModel),
                "MultipleChoice" => MultipleChoiceComponent.Create(component, dataModel),
                "DateTimeInput" => DateTimeInputComponent.Create(component, dataModel),
                "Divider" => DividerComponent.Create(component, dataModel),
                "List" => ListComponent.Create(component, dataModel),
                "Tabs" => TabsComponent.Create(component, dataModel),
                "Modal" => ModalComponent.Create(component, dataModel),
                "Video" => VideoComponent.Create(component, dataModel),
                "Audio" => AudioComponent.Create(component, dataModel),
                _ => CreateFallbackComponent(component)
            };
            
            if (element != null)
            {
                // Apply common styling
                ApplyCommonStyles(element, component);
            }
            
            return element;
        }
        
        private VisualElement CreateFallbackComponent(ComponentNode component)
        {
            var container = new VisualElement();
            container.AddToClassList("a2ui-unknown-component");
            
            var label = new Label($"Unknown component: {component.Type}");
            label.AddToClassList("a2ui-error");
            container.Add(label);
            
            Debug.LogWarning($"A2UI: Unknown component type '{component.Type}' for component '{component.Id}'");
            
            return container;
        }
        
        private void ApplyCommonStyles(VisualElement element, ComponentNode component)
        {
            // Add component type class
            element.AddToClassList($"a2ui-{component.Type.ToLower()}");
            
            // Apply weight for flex layout
            if (component.Weight.HasValue)
            {
                element.style.flexGrow = component.Weight.Value;
            }
            
            // Apply custom class names if provided
            if (component.ClassName != null && component.ClassName.Count > 0)
            {
                foreach (var className in component.ClassName)
                {
                    element.AddToClassList(className);
                }
            }
        }
    }
}
