using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MenuSongFactory 
{
    public static MainMenuSong getInstance(TextAsset songJson, AudioClip audioClip)
    {
        SongHeader song = JsonUtility.FromJson<SongHeader>(songJson.text);
        return new MainMenuSong(song, audioClip);
    }
}
