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
using A2UI.Unity.Utils;

namespace A2UI.Unity.Components
{
    /// <summary>
    /// Renders A2UI Text components as Unity Labels.
    /// </summary>
    public static class TextComponent
    {
        public static VisualElement Create(ComponentNode component, DataModel dataModel)
        {
            var label = new Label();
            label.AddToClassList("a2ui-text");
            
            // Get text property
            var textValue = PropertyHelper.GetStringValue(component, "text", dataModel);
            
            // Get usage hint
            var usageHint = component.GetProperty<string>("usageHint", "body");
            label.AddToClassList($"a2ui-text-{usageHint}");
            
            // Process markdown for basic formatting
            var processedText = MarkdownHelper.ProcessSimpleMarkdown(textValue, usageHint);
            label.text = processedText;
            
            // Enable rich text for basic formatting
            label.enableRichText = true;
            
            return label;
        }
    }
}
