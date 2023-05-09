using System.Collections.Generic;
using UnityEngine;

public class ObjectPrefabManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectPrefabs;

    private Dictionary<string, GameObject> objectPrefabMap;

    public static ObjectPrefabManager Manager { get; private set; }



    private void Awake()
    {
        // Ensure that there's only one instance of ObjectPrefabManager
        if (Manager != null && Manager != this)
        {
            Destroy(this);
        }
        else
        {
            Manager = this;
        }

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

    /// <summary>
    /// Gets prefab by type name
    /// </summary>
    /// <param name="name">The name of the prefab to look up</param>
    /// <returns>The prefab's GameObject if found, null if not</returns>
    public GameObject GetPrefabByType(string name)
    {
        if (name == null)
        {
            Debug.LogWarning("Cannot get prefab with null name");
            return null;
        }
       if (objectPrefabMap.ContainsKey(name))
       {
            return objectPrefabMap[name];
       }
       else
       {
            return null;
       }
    }

    /// <summary>
    /// Check if a prefab exists
    /// </summary>
    /// <param name="name">The name of the prefab to look up</param>
    /// <returns>True if found, false if not</returns>
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
        if (!objectPrefabMap.ContainsKey(obj.name))
        {
            objectPrefabs.Add(obj);
            objectPrefabMap.Add(obj.name, obj);
            // Prevents the prefab from showing up in scene
            obj.SetActive(false);
            obj.hideFlags = HideFlags.HideInHierarchy;
        }
    }   
}
