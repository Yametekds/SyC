using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SongWithDetails : SongHeader
{
    [NonSerialized]
    private int currentIndex = 0;
    public List<DataNote> notes;

    
    public DataNote PopNote() 
    {
        DataNote note = null;
        if (currentIndex >= 0 && currentIndex < notes.Count) 
        {
            note = this.notes[currentIndex];
            
            note.id = currentIndex;
            note.hitOffset = this.hitOffset;
            note.isProcess = false;
#if UNITY_EDITOR
           // Debug.Log("note index: " + currentIndex + " note time: " + note.timeMoment);
#endif
            currentIndex++;
        }
        return note;
    }

    public bool SpawnNote(float currentTime) 
    {
        bool play = false;
        
        if (currentIndex >= 0 && currentIndex < notes.Count)
        {
            DataNote note = this.notes[currentIndex];
            play = currentTime >= (note.spawnTime);
        }
        return play;
    }

    public bool IsTheEnd()
    {
        return currentIndex >= notes.Count;
    }

    public void Reset()
    {
        currentIndex = 0;
    }
}
