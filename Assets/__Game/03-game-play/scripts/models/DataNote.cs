using System;
using UnityEngine;

[Serializable]
public class DataNote 
{
    #region
    [NonSerialized]
    public int id;
    [NonSerialized]
    public float hitOffset;
    [NonSerialized]
    public bool isProcess;
    #endregion
    public int key;
    public float spawnTime;
    
}
