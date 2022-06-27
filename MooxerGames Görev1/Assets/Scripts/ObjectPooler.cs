using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, List<GameObject>> poolDictionary;
    
    public List<GameObject> pooledObjects;

    #region Singleton

    public static ObjectPooler SharedInstance;

    void Awake()
    {
        SharedInstance = this;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, List<GameObject>>();

        foreach (Pool pool in pools)
        {
            pooledObjects = new List<GameObject>();

            // Loop through list of pooled objects,deactivating them and adding them to the list 
            // pooledObjects = new List<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = (GameObject)Instantiate(pool.prefab);
                obj.SetActive(false);
                pooledObjects.Add(obj);
                obj.transform.SetParent(this.transform); // set as children of Spawn Manager
            }

            poolDictionary.Add(pool.tag, pooledObjects);
        }

        
    }

    public GameObject GetPooledObject(string tag)
    {
        var pooledObjects = poolDictionary[tag];

        // For as many objects as are in the pooledObjects list
        for (int i = 0; i < pooledObjects.Count; i++)
        {            
            // if the pooled objects is NOT active, return that object 
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        // otherwise, return null   
        return null;
    }

    public GameObject SpawnFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag" + tag + "doesn't excist.");
            return null;
        }

        GameObject objectToSpawn = GetPooledObject(tag);
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }
}
