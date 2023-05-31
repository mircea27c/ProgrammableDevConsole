# ProgrammableDevConsole
Free programmable in-game console for Unity

Instructions

To create a console command you must add [ConsoleCommand("CommandName")] attribute above your method.

For the command to work the method must:
-Be public
-Be static
-Have simple type parameters(float, int, string)

Example of working command:

[ConsoleCommand("teleport")]
public static void TeleportPlayer(float x, float y, float z){
	Vector3 destination = new Vector3(x,y,z);
	myPlayer.transform.position = destination;
}

usage in console:
teleport 436.3 12.2 532



Example of wrong commands:

[ConsoleCommand("delete")]
public static void DeleteItem(GameObject item){
	//code to delete item
}
problem: parameter can't be GameObject

[ConsoleCommand("delete")]
private void DeleteItem(string itemId){
	//code to delete item
}
problem: method must be static and public

