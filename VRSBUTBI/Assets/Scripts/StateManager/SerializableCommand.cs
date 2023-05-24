using UnityEngine;
using System.Collections.Generic;
using SerializableHelper;

[System.Serializable]
public class SerializableCommand
{
    public SerializableList<string> command;

    public SerializableCommand(object[] cmd)
    {
        command = new SerializableList<string>();
        foreach (var part in cmd)
        {
            command.list.Add(part.ToString());
        }
    }

    public object[] ToObjectData()  
    {
        List<object> data = new List<object>();
        foreach(var part in command.list)
        {
            data.Add(part);
        }
        return data.ToArray();
    }
}
