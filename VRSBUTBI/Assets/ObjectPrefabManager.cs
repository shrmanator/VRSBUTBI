using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a collection of GameObject prefabs.
/// </summary>
public class ObjectPrefabManager : MonoBehaviour
{
    /// <summary>
    /// List of GameObject prefabs.
    /// </summary>
    [SerializeField] private List<GameObject> objectPrefabs;

    /// <summary>
    /// Map of GameObject prefabs keyed by name.
    /// </summary>
    private Dictionary<string, GameObject> objectPrefabMap;

    /// <summary>
    /// Singleton instance of the ObjectPrefabManager.
    /// </summary>
    public static ObjectPrefabManager Manager { get; private set; }



    /// <summary>
    /// Initialize the ObjectPrefabManager and populate the prefab map.
    /// </summary>
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

    /// <summary>
    /// Adds a new GameObject to the prefab list and map.
    /// </summary>
    /// <param name="obj">The GameObject to add</param>
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
