using UnityEngine;
using System.Collections.Generic;
using SerializableHelper;

[System.Serializable]
public class SerializableScene
{
    public SerializableList<SerializableGameObject> objects;
    public SerializableList<SerializablePath> paths;
    public SerializableList<SerializableCommand> commands;

    public SerializableScene()
    {
        objects = new SerializableList<SerializableGameObject>();
        paths = new SerializableList<SerializablePath>();
        commands = new SerializableList<SerializableCommand>();
    }

    public SerializableScene(List<SerializableGameObject> newObjects, List<SerializablePath> newPaths, List<SerializableCommand> newCommands)
    {
        objects = new SerializableList<SerializableGameObject>(newObjects);
        paths = new SerializableList<SerializablePath>(newPaths);
        commands = new SerializableList<SerializableCommand>(newCommands);
    }

    public void AddObject(string objectName, string objectType, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        objects.list.Add(new SerializableGameObject(objectName, objectType, position, rotation, scale));
    }

    public void AddPath(List<Vector3> path)
    {
        paths.list.Add(new SerializablePath(path));
    }

    public void AddCommand(object[] cmd)
    {
        commands.list.Add(new SerializableCommand(cmd));
    }
}
