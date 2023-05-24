using UnityEngine;
/**
This script defines two custom serializable classes, SerializableGameObject and SerializableVector3, 
that can be used to serialize and deserialize Unity game objects and their position and rotation vectors. 

It also includes a custom deserialization binder, VersionDeserializationBinder, that can be used to deserialize
older versions of serialized objects that may have been modified since their initial serialization.

Serialized game objects, in this case, are saved to and loaded from files using the "SimpleFileBrowser" package.

This script contains the following methods and classes:
- SerializableGameObject: a class that defines a custom serializable game object
- SerializableVector3: a class that defines a custom serializable Vector3
- VersionDeserializationBinder: a class that defines a custom deserialization binder for older versions of serialized objects

NOTE: this may appear to be a long script; the commenting make it seem longer than it is.
**/

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
