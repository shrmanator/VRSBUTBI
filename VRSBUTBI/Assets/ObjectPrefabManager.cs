using System.Collections.Generic;
using UnityEngine;

public class ObjectPrefabManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectPrefabs;

    private Dictionary<string, GameObject> objectPrefabMap;

    public static ObjectPrefabManager Manager { get; private set; }



    private void Awake()
    {
        if (Manager != null && Manager != this)
        {
            Destroy(this);
        }
        else
        {
            Manager = this;
        }

        Debug.Log("OPM Awake");
        objectPrefabMap = new Dictionary<string, GameObject>();

        if (objectPrefabs == null || objectPrefabs.Count == 0)
        {
            Debug.LogWarning("ObjectPrefabs list is empty or not assigned.");
            return;
        }

        foreach (GameObject prefab in objectPrefabs)
        {
            objectPrefabMap.Add(prefab.name, prefab);
        }
    }

    public GameObject GetPrefabByType(string type)
    {
        Debug.Log(objectPrefabMap.Count);
        if (type == null)
        {
            Debug.LogWarning("Cannot get prefab with null name");
            return null;
        }
        Debug.Log("Getting type: " + type);
        Debug.Log("Contains type: " + objectPrefabMap.ContainsKey(type));
       if (objectPrefabMap.ContainsKey(type))
       {
            Debug.Log("Returning " + objectPrefabMap[type]);
            return objectPrefabMap[type];
       }
       else
       {
            return null;
       }
    }

    public bool HasPrefab(string name)
    {
        return objectPrefabMap.ContainsKey(name);
    }

    public void AddObjectToPrefabList(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogWarning("Cannot add null object to ObjectPrefabs list.");
            return;
        }

        objectPrefabs.Add(obj);
        objectPrefabMap.Add(obj.name, obj);
        obj.SetActive(false);
        obj.hideFlags = HideFlags.HideInHierarchy;
    }
}
