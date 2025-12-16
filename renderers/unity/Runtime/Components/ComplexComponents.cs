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
    /// Renders A2UI List components.
    /// </summary>
    public static class ListComponent
    {
        public static VisualElement Create(ComponentNode component, DataModel dataModel)
        {
            var listView = new ListView();
            listView.AddToClassList("a2ui-list");
            
            // TODO: Set up dynamic item source from data model
            
            return listView;
        }
    }
    
    /// <summary>
    /// Renders A2UI Tabs components.
    /// </summary>
    public static class TabsComponent
    {
        public static VisualElement Create(ComponentNode component, DataModel dataModel)
        {
            var container = new VisualElement();
            container.AddToClassList("a2ui-tabs");
            
            // TODO: Create tab headers and content areas
            
            return container;
        }
    }
    
    /// <summary>
    /// Renders A2UI Modal components.
    /// </summary>
    public static class ModalComponent
    {
        public static VisualElement Create(ComponentNode component, DataModel dataModel)
        {
            var modal = new VisualElement();
            modal.AddToClassList("a2ui-modal");
            
            // Overlay background
            modal.style.position = Position.Absolute;
            modal.style.left = 0;
            modal.style.right = 0;
            modal.style.top = 0;
            modal.style.bottom = 0;
            
            // Modal content container
            var content = new VisualElement();
            content.AddToClassList("a2ui-modal-content");
            modal.Add(content);
            
            return modal;
        }
    }
    
    /// <summary>
    /// Renders A2UI MultipleChoice components.
    /// </summary>
    public static class MultipleChoiceComponent
    {
        public static VisualElement Create(ComponentNode component, DataModel dataModel)
        {
            var container = new VisualElement();
            container.AddToClassList("a2ui-multiple-choice");
            
            // TODO: Create radio buttons or checkboxes based on selection mode
            
            return container;
        }
    }
    
    /// <summary>
    /// Renders A2UI DateTimeInput components.
    /// </summary>
    public static class DateTimeInputComponent
    {
        public static VisualElement Create(ComponentNode component, DataModel dataModel)
        {
            var textField = new TextField();
            textField.AddToClassList("a2ui-datetime-input");
            
            // TODO: Add date/time picker functionality
            
            return textField;
        }
    }
}
