using System.Runtime.CompilerServices;
using System.Globalization;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Reflection;
using System;


namespace SerializableHelper
{
/// <summary>
/// A serializable version of Unity's Vector3 class.
/// </summary>
[System.Serializable]
public class SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    /// <summary>
    /// Converts the serializable vector to a Unity Vector3 object.
    /// </summary>
    /// <returns>A Vector3 object with the same values as the serializable vector.</returns>
    public Vector3 ToVector3()
    {
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

[System.Serializable]

public class SerializableList<T> : List<T>
{
    public List<T> list;

    public SerializableList()
    {
        list = new List<T>();
    }

    public SerializableList(List<T> list)
    {
        this.list = list;
    }
}
}