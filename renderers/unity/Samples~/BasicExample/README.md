# A2UI Unity Renderer - Basic Example

This example demonstrates how to use the A2UI Unity renderer in a Unity project.

## Setup

1. Add the `BasicA2UIExample.cs` script to a GameObject in your scene
2. Create a UIDocument component on the same GameObject
3. Assign the UIDocument reference in the inspector
4. Run the scene

## What it does

The example:
- Creates an A2UI renderer instance
- Processes example A2UI messages to create a simple UI
- Displays a welcome message with a button
- Logs user interactions to the Unity console

## Next Steps

To integrate with an actual agent:
1. Replace `ProcessExampleMessage()` with real A2UI message processing
2. Connect to an agent via A2A protocol or other transport
3. Send user actions back to the agent
4. Handle streaming messages for real-time UI updates
