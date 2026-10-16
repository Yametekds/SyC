using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatInfo
{
    private readonly float bpm;
    private readonly float bps;
    private readonly float secondsBetweenBeats;

    public BeatInfo(float bpm)
    {
        this.bpm = bpm;
        this.bps = bpm / 60;
        this.secondsBetweenBeats = 1 / bps;
    }

    public float Bpm {get { return this.bpm; } }

    public float Bps { get { return this.bps; } }

    public float SecondsBetweenBeats { get { return this.secondsBetweenBeats; } }


}
