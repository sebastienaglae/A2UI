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
    /// Renders A2UI Column components.
    /// </summary>
    public static class ColumnComponent
    {
        public static VisualElement Create(ComponentNode component, DataModel dataModel)
        {
            var container = new VisualElement();
            container.AddToClassList("a2ui-column");
            container.style.flexDirection = FlexDirection.Column;
            
            return container;
        }
    }
    
    /// <summary>
    /// Renders A2UI Row components.
    /// </summary>
    public static class RowComponent
    {
        public static VisualElement Create(ComponentNode component, DataModel dataModel)
        {
            var container = new VisualElement();
            container.AddToClassList("a2ui-row");
            container.style.flexDirection = FlexDirection.Row;
            
            return container;
        }
    }
    
    /// <summary>
    /// Renders A2UI Card components.
    /// </summary>
    public static class CardComponent
    {
        public static VisualElement Create(ComponentNode component, DataModel dataModel)
        {
            var card = new VisualElement();
            card.AddToClassList("a2ui-card");
            
            return card;
        }
    }
    
    /// <summary>
    /// Renders A2UI Divider components.
    /// </summary>
    public static class DividerComponent
    {
        public static VisualElement Create(ComponentNode component, DataModel dataModel)
        {
            var divider = new VisualElement();
            divider.AddToClassList("a2ui-divider");
            divider.style.height = 1;
            
            return divider;
        }
    }
}
