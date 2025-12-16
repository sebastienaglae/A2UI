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

using UnityEngine.UIElements;
using A2UI.Unity.Core;

namespace A2UI.Unity.Components
{
    /// <summary>
    /// Renders A2UI Button components as Unity Buttons.
    /// </summary>
    public static class ButtonComponent
    {
        public static VisualElement Create(ComponentNode component, DataModel dataModel)
        {
            var button = new Button();
            button.AddToClassList("a2ui-button");
            
            // Button text from properties or children will be handled by parent
            // The action will be set up by the event system
            
            return button;
        }
    }
    
    /// <summary>
    /// Renders A2UI TextField components.
    /// </summary>
    public static class TextFieldComponent
    {
        public static VisualElement Create(ComponentNode component, DataModel dataModel)
        {
            var textField = new TextField();
            textField.AddToClassList("a2ui-textfield");
            
            // TODO: Set up data binding for value updates
            
            return textField;
        }
    }
    
    /// <summary>
    /// Renders A2UI Checkbox components as Unity Toggle.
    /// </summary>
    public static class CheckboxComponent
    {
        public static VisualElement Create(ComponentNode component, DataModel dataModel)
        {
            var toggle = new Toggle();
            toggle.AddToClassList("a2ui-checkbox");
            
            return toggle;
        }
    }
    
    /// <summary>
    /// Renders A2UI Slider components.
    /// </summary>
    public static class SliderComponent
    {
        public static VisualElement Create(ComponentNode component, DataModel dataModel)
        {
            var slider = new Slider();
            slider.AddToClassList("a2ui-slider");
            
            return slider;
        }
    }
}
