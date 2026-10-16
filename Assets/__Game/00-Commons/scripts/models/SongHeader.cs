using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SongHeader
{
    public string name;
    public string author;
    public float mainBpm;
    public float hitOffset;

    public string GetName()
    {
        return name;
    }

    public string GetAuthor()
    {
        return author;
    }

    public float GetMainBpm()
    {
        return mainBpm;
    }
    
    public float GetHitOffset()
    {
        return hitOffset;
    }


}

