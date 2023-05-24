using UnityEngine;
using System.Runtime.Serialization;
using SerializableHelper;

/// <summary>
/// Class for serializing and deserializing game objects.
/// </summary>

[System.Serializable]
public class SerializableGameObject {
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
        this.scale = scale;
    }
    

    public SerializableGameObject(string objectName, string objectType, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        this.objectName = objectName;
        this.objectType = objectType;
        this.position = new SerializableVector3(position);
        this.rotation = new SerializableVector3(rotation);
        this.scale = new SerializableVector3(scale);
    }


    /// <summary>
    /// Method for converting SerializableGameObject to an object array in the form used by ObjectManager
    /// </summary>
    /// <returns> an object array in the form used by ObjectManager to create objects </returns>
    public object[] ToObjectData()
    {
        object[] objectData = {objectName, objectType, position.x, position.y, position.z, rotation.x, rotation.y, rotation.z,};
        return objectData;
    }


protected SerializableGameObject(SerializationInfo info, StreamingContext context)
    {
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