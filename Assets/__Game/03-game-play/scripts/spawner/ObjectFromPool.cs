using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a component that simply identifies the pool that a GameObject came from
public class ObjectFromPool : MonoBehaviour
{
    public PoolOfObjects SoucePool { get; set; }

    public void ReturnToPool() 
    { 
        if(SoucePool != null) 
        {
            SoucePool.ReturnObject(gameObject);
        }
    }
}
