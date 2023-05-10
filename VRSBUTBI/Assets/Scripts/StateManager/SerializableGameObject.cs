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

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using SimpleFileBrowser;
using System.Runtime.Serialization;
using System.Reflection;
using System;

/// <summary>
/// Class for serializing and deserializing game objects.
/// </summary>
[System.Serializable]
public class SerializableGameObject
{
    public string objectName;
    public string objectType;
    public SerializableVector3 position;
    public SerializableVector3 rotation;
    public SerializableVector3 scale;

    public SerializableGameObject(string objectName, string objectType, SerializableVector3 position, SerializableVector3 rotation, SerializableVector3 scale)
    {
        this.objectName = objectName;
        this.objectType = objectType;
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;;
    }

    protected SerializableGameObject(SerializationInfo info, StreamingContext context)
    {
        // ...
        position = (SerializableVector3)info.GetValue("position", typeof(SerializableVector3));
        rotation = (SerializableVector3)info.GetValue("rotation", typeof(SerializableVector3));

        // Deserialize the scale field
        scale = (SerializableVector3)info.GetValue("scale", typeof(SerializableVector3));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("name", objectName);
        info.AddValue("type", objectType);
        info.AddValue("position", position);
        info.AddValue("rotation", rotation);
        info.AddValue("scale", scale);
    }

    /// <summary>
    /// Method for converting SerializableGameObject to an object array in the form used by ObjectManager
    /// </summary>
    /// <returns> an object array in the form used by ObjectManager to create objects </returns>
    public object[] ToObjectData()
    {
        object[] objectData = {objectName, objectType, 
            position.x, position.y, position.z,
            rotation.x, rotation.y, rotation.z};
        return objectData;
    }

    /// <summary>
    /// Method for deserializing a SerializableGameObject into this instance.
    /// </summary>
    /// <param name="obj">The SerializableGameObject to deserialize.</param>
    public void Deserialize(object obj)
    {
        if (obj is SerializableGameObject serializedObject)
        {
            objectName = serializedObject.objectName;
            objectType = serializedObject.objectType;
            position = serializedObject.position;
            rotation = serializedObject.rotation;
        }
    }
}


/// <summary>
/// A serializable version of Unity's Vector3 class.
/// </summary>
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

    /// <summary>
    /// Converts the serializable vector to a Unity Vector3 object.
    /// </summary>
    /// <returns>A Vector3 object with the same values as the serializable vector.</returns>
    public Vector3 ToVector3() {
        return new Vector3(x, y, z);
    }
}

/// <summary>
/// A custom binder for deserializing Unity objects with specific versions.
/// </summary>
public class VersionDeserializationBinder : SerializationBinder
{
    /// <summary>
    /// Binds the given type name to the corresponding Type object and returns it.
    /// </summary>
    /// <param name="assemblyName">The name of the assembly that the type belongs to.</param>
    /// <param name="typeName">The name of the type to bind.</param>
    /// <returns>The Type object that corresponds to the given type name.</returns>
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
