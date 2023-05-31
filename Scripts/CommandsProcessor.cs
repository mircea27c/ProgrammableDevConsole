using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
public class CommandsProcessor : MonoBehaviour
{
    private Dictionary<string, MethodInfo> commands;
    private Dictionary<string, Type[]> commandParameterTypes;

    private void Start()
    {
        RegisterCommands();
    }

    private void RegisterCommands()
    {
        commands = new Dictionary<string, MethodInfo>();
        commandParameterTypes = new Dictionary<string, Type[]>();

        Type[] scriptTypes = Assembly.GetExecutingAssembly().GetTypes();

        foreach (Type scriptType in scriptTypes)
        {
            MethodInfo[] methods = scriptType.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (MethodInfo method in methods)
            {
                ConsoleCommandAttribute attribute = Attribute.GetCustomAttribute(method, typeof(ConsoleCommandAttribute)) as ConsoleCommandAttribute;
                if (attribute != null)
                {
                    commands[attribute.commandName] = method;
                    ParameterInfo[] parameters = method.GetParameters();
                    Type[] parameterTypes = new Type[parameters.Length];
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameterTypes[i] = parameters[i].ParameterType;
                    }
                    commandParameterTypes[attribute.commandName] = parameterTypes;
                }
            }
        }
    }

    public void ProcessCommand(string input)
    {
        print($"[Command] {input}");

        string[] inputParts = input.Split(' ');
        string command = inputParts[0];

        if (commands.ContainsKey(command))
        {
            MethodInfo method = commands[command];
            Type[] parameterTypes = commandParameterTypes[command];

            foreach (Type param in parameterTypes)
            {
                if (!IsValidType(param)) {
                    Debug.LogWarning($"'{command}' command was set up incorrectly. Parameters must be float, int or string. Found wrong parameter of type {param.Name}");
                    return;
                }
            }

            if (parameterTypes.Length == 0)
            {
                method.Invoke(null, null);
            }
            else
            {
                object[] parameters = ParseParameters(inputParts, parameterTypes);
                if (parameters != null)
                {
                    method.Invoke(null, parameters);
                }
                else
                {
                    Debug.LogWarning("Invalid parameters for command: " + command);
                }
            }
        }
        else
        {
            Debug.LogWarning("Command not found: " + command);
        }
    }
    bool IsValidType(Type param) {
        if (param== typeof(int) || param == typeof(char) || param== typeof(float) || param == typeof(string)) {
            return true;
        }
        return false;
    }
    private object[] ParseParameters(string[] inputParts, Type[] parameterTypes)
    {
        if (inputParts.Length - 1 != parameterTypes.Length)
        {
            return null;
        }

        object[] parameters = new object[parameterTypes.Length];
        for (int i = 0; i < parameterTypes.Length; i++)
        {
            Type parameterType = parameterTypes[i];
            string inputPart = inputParts[i + 1];
            object parameter = null;

            if (parameterType == typeof(int))
            {
                int.TryParse(inputPart, out int value);
                parameter = value;
            }
            else if (parameterType == typeof(float))
            {
                float.TryParse(inputPart, out float value);
                parameter = value;
            }
            else if (parameterType == typeof(string))
            {
                parameter = inputPart;
            }
            else if (parameterType == typeof(bool)) {
                bool.TryParse(inputPart, out bool value);
                parameter = value;
            }

            if (parameter == null)
            {
                return null;
            }

            parameters[i] = parameter;
        }

        return parameters;
    }
}
