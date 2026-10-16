using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LevelModel 
{
    private AudioClip baseClip;
    private SongWithDetails song;
    private IList<CeremonyArc> ceremonyArcs;

    public void SetBaseClip(AudioClip baseClip) 
    {
        this.baseClip = baseClip;
    }

    public AudioClip GetBaseClip()
    {
        return this.baseClip;
    }

    public void SetSongWithDetails(TextAsset jsonSong) 
    { 
        this.song = JsonUtility.FromJson<SongWithDetails>(jsonSong.text);
    }

    public void SetSongWithDetails(SongWithDetails song)
    {
        this.song = song;
    }

    public SongWithDetails GetSongWithDetails()
    {
        return this.song;
    }

    public void SetCeremonyArcs(IList<CeremonyArc> ceremonyArcs)
    {
        this.ceremonyArcs = ceremonyArcs;
    }

    public IList<CeremonyArc> GetCeremonyArcs()
    {
        return this.ceremonyArcs;
    }
}
