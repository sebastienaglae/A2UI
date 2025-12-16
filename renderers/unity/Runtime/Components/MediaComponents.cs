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

namespace A2UI.Unity.Components
{
    /// <summary>
    /// Renders A2UI Image components.
    /// </summary>
    public static class ImageComponent
    {
        public static VisualElement Create(ComponentNode component, DataModel dataModel)
        {
            var image = new Image();
            image.AddToClassList("a2ui-image");
            
            // TODO: Load image from URL asynchronously
            // For now, just create the placeholder
            
            return image;
        }
    }
    
    /// <summary>
    /// Renders A2UI Icon components.
    /// </summary>
    public static class IconComponent
    {
        public static VisualElement Create(ComponentNode component, DataModel dataModel)
        {
            var icon = new Image();
            icon.AddToClassList("a2ui-icon");
            
            // TODO: Map icon names to Unity sprites/textures
            
            return icon;
        }
    }
    
    /// <summary>
    /// Renders A2UI Video components.
    /// </summary>
    public static class VideoComponent
    {
        public static VisualElement Create(ComponentNode component, DataModel dataModel)
        {
            var container = new VisualElement();
            container.AddToClassList("a2ui-video");
            
            // TODO: Integrate Unity VideoPlayer
            var label = new Label("Video Player");
            container.Add(label);
            
            return container;
        }
    }
    
    /// <summary>
    /// Renders A2UI Audio components.
    /// </summary>
    public static class AudioComponent
    {
        public static VisualElement Create(ComponentNode component, DataModel dataModel)
        {
            var container = new VisualElement();
            container.AddToClassList("a2ui-audio");
            
            // TODO: Integrate Unity AudioSource
            var label = new Label("Audio Player");
            container.Add(label);
            
            return container;
        }
    }
}
