# A2UI Unity UI Toolkit Renderer

Unity UI Toolkit (UXML) implementation of the A2UI protocol.

## Overview

This renderer enables Unity applications to display agent-generated UIs using Unity's UI Toolkit system. It converts A2UI JSON messages into UXML (Unity XML) elements and manages data binding and event handling.

## Features

- **Native Unity UI Toolkit Support**: Uses Unity's declarative UXML format
- **Component Catalog**: Maps A2UI components to Unity UI elements
- **Data Binding**: Dynamic updates to UI elements based on data model changes
- **Event Handling**: User interactions flow back to agents via A2A protocol
- **Progressive Rendering**: Supports streaming A2UI messages

## Requirements

- Unity 2021.3 LTS or higher (UI Toolkit available)
- .NET Standard 2.1 or .NET 6+

## Installation

### Unity Package Manager

1. Open Unity Package Manager
2. Click "+" and select "Add package from git URL"
3. Enter: `https://github.com/google/A2UI.git?path=/renderers/unity`

### Manual Installation

1. Download or clone this repository
2. Copy the `renderers/unity/Runtime` folder to your Unity project's `Assets` folder
3. Copy the `renderers/unity/package.json` to define the package (optional)

## Quick Start

### Basic Setup

```csharp
using A2UI.Unity;
using UnityEngine;
using UnityEngine.UIElements;

public class A2UIExample : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    private A2UIRenderer renderer;

    void Start()
    {
        // Create the renderer
        renderer = new A2UIRenderer(uiDocument.rootVisualElement);
        
        // Process A2UI messages
        string a2uiMessage = GetA2UIMessage(); // Your message source
        renderer.ProcessMessage(a2uiMessage);
    }
}
```

### Processing Messages

```csharp
// Handle surface updates
renderer.OnSurfaceUpdate += (surfaceId, rootElement) =>
{
    Debug.Log($"Surface {surfaceId} updated");
};

// Handle user actions
renderer.OnUserAction += (action) =>
{
    // Send action back to agent via A2A protocol
    SendToAgent(action);
};

// Process individual JSONL messages
foreach (var line in jsonlStream.Split('\n'))
{
    renderer.ProcessMessage(line);
}
```

## Component Mapping

A2UI components map to Unity UI Toolkit elements as follows:

| A2UI Component | Unity UI Toolkit Element | Notes |
|----------------|--------------------------|-------|
| Text | Label | Supports markdown via rich text |
| Button | Button | Click events mapped to actions |
| TextField | TextField | Two-way data binding |
| Image | Image | URL-based image loading |
| Icon | Image | Material icons as sprites |
| Column | VisualElement (vertical) | Flexbox column layout |
| Row | VisualElement (horizontal) | Flexbox row layout |
| Card | VisualElement | Custom styling for cards |
| Checkbox | Toggle | Boolean state binding |
| Slider | Slider | Numeric value binding |
| MultipleChoice | RadioButtonGroup | Single/multiple selection |
| DateTimeInput | TextField | Date/time formatting |
| Divider | VisualElement | Horizontal separator |
| List | ListView | Dynamic item rendering |
| Tabs | TabView | Tab navigation |
| Modal | VisualElement | Overlay/dialog support |
| Video | Custom VisualElement | Video player integration |
| Audio | Custom VisualElement | Audio player integration |

## Theming

Unity UI Toolkit uses USS (Unity Style Sheets) for styling. The renderer applies theme classes from A2UI messages to Unity elements.

### Custom Theme Example

```csharp
// Apply custom USS stylesheet
var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/MyA2UITheme.uss");
renderer.ApplyStyleSheet(styleSheet);
```

### USS Example

```css
/* MyA2UITheme.uss */
.a2ui-button {
    background-color: rgb(0, 122, 255);
    color: white;
    border-radius: 4px;
    padding: 8px 16px;
}

.a2ui-card {
    background-color: white;
    border-radius: 8px;
    padding: 16px;
    margin: 8px;
}

.a2ui-text-h1 {
    font-size: 24px;
    -unity-font-style: bold;
}
```

## Advanced Usage

### Custom Components

You can register custom component renderers:

```csharp
renderer.RegisterCustomComponent("MyCustomType", (component, dataModel) =>
{
    var element = new VisualElement();
    element.AddToClassList("custom-component");
    // Custom rendering logic
    return element;
});
```

### Data Model Updates

```csharp
// Listen for data model changes
renderer.OnDataModelUpdate += (surfaceId, path, value) =>
{
    Debug.Log($"Data updated at {path}: {value}");
};
```

### Error Handling

```csharp
renderer.OnError += (error) =>
{
    Debug.LogError($"A2UI Error: {error.Message}");
    // Optionally send error back to agent
};
```

## Examples

See the [samples](../../samples) directory for complete examples:

- Basic chat interface with A2UI
- Restaurant finder demo
- Form generation example

## Architecture

The renderer consists of several key components:

- **A2UIRenderer**: Main class that processes A2UI messages
- **ComponentFactory**: Creates Unity UI elements from A2UI component definitions
- **DataBinder**: Manages data binding between data model and UI elements
- **EventHandler**: Captures user interactions and formats them as A2A messages
- **MessageProcessor**: Parses JSONL messages and updates surfaces

## Limitations

- Some advanced markdown features may not be supported in Unity's rich text
- Video and audio components require Unity's VideoPlayer and AudioSource components
- Complex animations should be handled through USS transitions
- Image loading from URLs requires UnityWebRequest (async operations)

## Security Considerations

⚠️ **Important**: As with all A2UI renderers, treat agent-generated content as untrusted:

- Validate all URLs before loading external resources
- Sanitize any user-provided strings before displaying
- Use Content Security Policy equivalents where applicable
- Limit embedded content capabilities
- Implement appropriate sandboxing for custom components

## Troubleshooting

### UI Not Rendering
- Ensure UIDocument is properly configured in your scene
- Check that the UXML root element exists
- Verify A2UI messages are valid JSONL format

### Styling Issues
- Make sure USS stylesheets are properly loaded
- Check that class names match between A2UI and USS
- Verify Unity UI Toolkit is enabled in Player Settings

### Performance Issues
- Use ListView for large lists instead of rendering many elements
- Enable UI Toolkit runtime batching
- Profile using Unity Profiler's UI module

## Contributing

Contributions are welcome! Please see the main [CONTRIBUTING.md](../../CONTRIBUTING.md) for guidelines.

## License

Apache 2.0 - See [LICENSE](../../LICENSE) for details.

## Resources

- [Unity UI Toolkit Documentation](https://docs.unity3d.com/Manual/UIElements.html)
- [A2UI Specification](../../specification/0.8/docs/a2ui_protocol.md)
- [A2A Protocol](https://github.com/google/a2a)
