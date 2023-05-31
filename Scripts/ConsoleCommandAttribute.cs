using System;

[AttributeUsage(AttributeTargets.Method)]
public class ConsoleCommandAttribute : Attribute
{
    public string commandName;
    public Type[] parameterTypes;
    public ConsoleCommandAttribute(string cmdName, params Type[] parameters) {
        this.commandName = cmdName;
        this.parameterTypes = parameters;
    }
}
