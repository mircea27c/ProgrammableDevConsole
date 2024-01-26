<div align=center>

# ProgrammableDevConsole

**ProgrammableDevConsole** is a free, in-game console for Unity that allows developers to create and execute custom commands. 

</div>

## Instructions

To create a console command, follow these steps:

1. Add the `[ConsoleCommand("CommandName")]` attribute above your method.
2. Ensure that the method meets the following criteria:
   - Be public
   - Be static
   - Have simple type parameters (float, int, string, bool)

### Example of a working command:

```csharp
[ConsoleCommand("teleport")]
public static void TeleportPlayer(float x, float y, float z){
    Vector3 destination = new Vector3(x, y, z);
    myPlayer.transform.position = destination;
}

```

Usage in the console: `teleport 436.3 12.2 532`

### Examples of incorrect commands:

```csharp
// Incorrect: Parameter can't be GameObject
[ConsoleCommand("delete")]
public static void DeleteItem(GameObject item){
    // Code to delete item
}

// Incorrect: Method must be static and public
[ConsoleCommand("delete")]
private void DeleteItem(string itemId){
    // Code to delete item
}
```
