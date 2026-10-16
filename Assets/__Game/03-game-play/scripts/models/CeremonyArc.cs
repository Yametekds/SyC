using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CeremonyArc 
{


    private AudioClip[] lowTone;
    private AudioClip[] middleTone;
    private AudioClip[] higthTone;
    private IList<BackgroundLayer> background;

    public void SetBackgound(IList<BackgroundLayer> background) 
    {
        this.background = background;
    }

    public IList<BackgroundLayer> GetBackground()
    {
        return this.background;
    }

    public void SetLowGamePlayTone(AudioClip[] lowTone)
    {
        this.lowTone = lowTone;
    }

    public AudioClip[] GetLowGamePlayTone()
    {
        return lowTone;
    }

    public void SetMidGamePlayTone(AudioClip[] middleTone)
    {
        this.middleTone = middleTone;
    }

    public AudioClip[] GetMidGamePlayTone()
    {
        return middleTone;
    }

    public void SetHigthGamePlayTone(AudioClip[] higthTone)
    {
        this.higthTone = higthTone;
    }

    public AudioClip[] GetHigthGamePlayTone()
    {
        return higthTone;
    }
}
