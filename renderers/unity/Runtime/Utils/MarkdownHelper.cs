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

using System.Text;
using System.Text.RegularExpressions;

namespace A2UI.Unity.Utils
{
    /// <summary>
    /// Helper for processing simple markdown to Unity rich text.
    /// </summary>
    public static class MarkdownHelper
    {
        /// <summary>
        /// Process simple markdown and convert to Unity rich text tags.
        /// Supports: bold, italic, headings (via usageHint).
        /// </summary>
        public static string ProcessSimpleMarkdown(string text, string usageHint = "body")
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            
            var result = text;
            
            // Handle headings based on usage hint
            if (usageHint == "h1")
                result = $"<size=24><b>{result}</b></size>";
            else if (usageHint == "h2")
                result = $"<size=20><b>{result}</b></size>";
            else if (usageHint == "h3")
                result = $"<size=18><b>{result}</b></size>";
            else if (usageHint == "h4")
                result = $"<size=16><b>{result}</b></size>";
            else if (usageHint == "h5")
                result = $"<size=14><b>{result}</b></size>";
            else if (usageHint == "caption")
                result = $"<i>{result}</i>";
            else
            {
                // Process inline markdown for body text
                // Process in order: bold first, then italic, to avoid conflicts
                
                // Bold: **text** or __text__
                result = Regex.Replace(result, @"\*\*(.+?)\*\*", "<b>$1</b>");
                result = Regex.Replace(result, @"__(.+?)__", "<b>$1</b>");
                
                // Italic: *text* or _text_ (but not if part of bold which is already replaced)
                // Use negative lookahead/lookbehind to avoid matching asterisks that are part of <b> tags
                result = Regex.Replace(result, @"(?<!<b>)\*(?!\*|>)(.+?)(?<!<)\*(?!</b>)", "<i>$1</i>");
                result = Regex.Replace(result, @"(?<!<b>)_(?!_|>)(.+?)(?<!<)_(?!</b>)", "<i>$1</i>");
                
                // Code: `text`
                result = Regex.Replace(result, @"`(.+?)`", "<color=#808080>$1</color>");
            }
            
            return result;
        }
    }
}
