using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using SimpleFileBrowser;
using System.Runtime.Serialization;
using System.Reflection;
using System;

[System.Serializable]
public class SerializableGameObject : ISerializable
{
    public string name;
    public SerializableVector3 position;
    public SerializableVector3 rotation;

    public SerializableGameObject(string name, SerializableVector3 position, SerializableVector3 rotation)
    {
        this.name = name;
        this.position = position;
        this.rotation = rotation;
    }

    // Constructor for deserialization
    protected SerializableGameObject(SerializationInfo info, StreamingContext context)
    {
        // Deserialize the object's fields from the SerializationInfo object
        name = info.GetString("name");
        position = (SerializableVector3)info.GetValue("position", typeof(SerializableVector3));
        rotation = (SerializableVector3)info.GetValue("rotation", typeof(SerializableVector3));
    }

    // Method to serialize the object's fields into a SerializationInfo object
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("name", name);
        info.AddValue("position", position);
        info.AddValue("rotation", rotation);
    }

    public void Deserialize(object obj)
    {
        if (obj is SerializableGameObject serializedObject)
        {
            name = serializedObject.name;
            position = serializedObject.position;
            rotation = serializedObject.rotation;
        }
    }
}

[System.Serializable]
public class SerializableVector3 {
    public float x;
    public float y;
    public float z;

    public SerializableVector3(Vector3 vector) {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3 ToVector3() {
        return new Vector3(x, y, z);
    }
}

public class VersionDeserializationBinder : SerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName)
    {
        Type typeToDeserialize = null;

        try
        {
            // Get the current assembly
            Assembly currentAssembly = Assembly.GetExecutingAssembly();

            // Attempt to load the specified type from the current assembly
            typeToDeserialize = currentAssembly.GetType(typeName);
        }
        catch (Exception)
        {
            // Ignore the exception and return null
        }

        return typeToDeserialize;
    }
}