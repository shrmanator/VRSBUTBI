/*
Handles the object-prefab mapping.
*/

using System.Collections.Generic;
using UnityEngine;


public class ObjectPrefabManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectPrefabs;

    public Dictionary<string, GameObject> objectPrefabMap;

    private void Awake()
    {
        objectPrefabMap = new Dictionary<string, GameObject>();

        foreach (GameObject prefab in objectPrefabs)
        {
            objectPrefabMap.Add(prefab.name, prefab);
        }
    }

    public GameObject GetPrefabByName(string name)
    {
        if (name == null)
        {
            Debug.LogWarning("Cannot get prefab with null name");
            return null;
        }
        return objectPrefabMap.ContainsKey(name) ? objectPrefabMap[name] : null;
    }

    public void AddObjectToPrefabList(GameObject obj)
    {
        objectPrefabs.Add(obj);
        objectPrefabMap.Add(obj.name, obj);
    }
    
}
