using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolOfObjects {

    public event Action OnObjectReturn;
    public GameObject Prefab { get; set; }
    public Transform Transform { get; set; }
    private Stack<GameObject> inactiveInstances = new Stack<GameObject>();

    public GameObject GetObject() 
    {
        
        GameObject spawnedGameObject;
        // if there is an inactive instance of the prefab ready to return, return that
        if (inactiveInstances.Count > 0)
        {
            // remove the instance from teh collection of inactive instances
            spawnedGameObject = inactiveInstances.Pop();
        }
        // otherwise, create a new instance
        else
        {
            spawnedGameObject = (GameObject)GameObject.Instantiate(Prefab);

            // add the PooledObject component to the prefab so we know it came from this pool
            ObjectFromPool objectFromPool = spawnedGameObject.AddComponent<ObjectFromPool>();
            objectFromPool.SoucePool = this;
        }
        spawnedGameObject.SetActive(false);
        // return a reference to the instance
        return spawnedGameObject;
    }

    public GameObject GetObjectToScene()
    {
        GameObject spawnedGameObject = GetObject();

        
        // put the instance in the root of the scene and enable it
        spawnedGameObject.transform.SetParent(null);
        spawnedGameObject.SetActive(true);

        // return a reference to the instance
        return spawnedGameObject;
    }

    public void ReturnObject(GameObject toReturn)
    {
        ObjectFromPool pooledObject = toReturn.GetComponent<ObjectFromPool>();

        // if the instance came from this pool, return it to the pool
        if (pooledObject != null && pooledObject.SoucePool == this)
        {
            // make the instance a child of this and disable it
            toReturn.transform.SetParent(Transform);
            toReturn.SetActive(false);

            // add the instance to the collection of inactive instances
            inactiveInstances.Push(toReturn);
        }
        // otherwise, just destroy it
        else
        {
            Debug.LogWarning(toReturn.name + " was returned to a pool it wasn't spawned from! Destroying.");
            GameObject.Destroy(toReturn);
        }

        OnObjectReturn?.Invoke();
    }
}
