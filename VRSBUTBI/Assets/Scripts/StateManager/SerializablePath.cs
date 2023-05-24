
using UnityEngine;
using System.Collections.Generic;
using SerializableHelper;

[System.Serializable]
public class SerializablePath
{
    public SerializableList<SerializableVector3> vertices;
    public SerializablePath(List<Vector3> path)
    {
        vertices = new SerializableList<SerializableVector3>();
        foreach(var vertex in path)
        {
            vertices.list.Add(new SerializableVector3(vertex));
        }
   }

    public List<Vector3> ToVerticesList()
    {
        List<Vector3> verticesList = new List<Vector3>();
        foreach (SerializableVector3 vertex in vertices.list)
        {
            verticesList.Add(vertex.ToVector3());
        }
        return verticesList;
    }
}
