using System;
using System.Collections;
using UnityEngine;

public abstract class BeatController : MonoBehaviour, IBeatController
{

    public event Action<BeatInfo> OnBeatChange;
    public event Action OnBeatSignal;
    private BeatInfo currentBeat;
    private float nextBeat = 0;
    private WaitUntil waitUntil;
    private bool couroutineLoop = false;

    public BeatInfo CurrentBeatInfo { get { return currentBeat; } }
    public float NextBeat { get { return nextBeat; } set { nextBeat = value; } }


    protected virtual void Awake()
    {
        this.couroutineLoop = true;
        waitUntil = new WaitUntil(()=> this.CurrentBeatInfo != null && BeatChecker());
        StartCoroutine(BeatCyclo());
    }

    protected virtual void OnDestroy()
    {
        this.couroutineLoop = false;
        StopAllCoroutines();
    }

    void Update()
    {

        if (this.CurrentBeatInfo == null)
        {
            return;
        }
        BeatUpdate();
    }


    public void SetBeatPerMinute(float bpm)
    {
        this.currentBeat = null;
        this.currentBeat = new BeatInfo(bpm);
        nextBeat = 0;
        OnBeatChange?.Invoke(this.currentBeat);
    }


    public virtual void BeatUpdate() 
    {
        this.NextBeat += Time.deltaTime;
    }

    public virtual bool BeatChecker()
    {
        return this.NextBeat >= this.currentBeat.SecondsBetweenBeats;
    }

    public virtual void BeatTrigger()
    {
        this.OnBeatSignal?.Invoke();
        this.NextBeat = 0;
    }

    private IEnumerator BeatCyclo() 
    {
        while (this.couroutineLoop) 
        {
            yield return waitUntil;
            BeatTrigger();
        }
    }
}
