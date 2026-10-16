using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class TimeController : MonoBehaviour, ITimeController, IGameHubDependecy
{
    private IGameController gameController;
    private bool playing = false;
    private float mTime = 0;
    private float mDeltaTime = 0;

    private static TimeController INSTANCE;
    private void Awake()
    {
        if(INSTANCE == null)
        {
            INSTANCE = this;
        }
        else
        {
            Destroy(this);
        }
    }

    

    private void OnDestroy()
    { 
        gameController.OnSongStart -= OnSongStart;
    }

    // Update is called once per frame
    void Update()
    {
        mDeltaTime = Time.deltaTime;
        if (!playing)
        {
            return;
        }
        
        mTime += mDeltaTime;
    }

    public static TimeController GetInstance() 
    {
        return INSTANCE;
    }
    public void SetInjection(IGameHubComponentController injection)
    {
        injection.OnLoadComplete += this.OnHubCompleteLoad;
    }

    private void OnHubCompleteLoad(IGameHubComponentController hub)
    {
        hub.OnLoadComplete -= this.OnHubCompleteLoad;
        SetGameController(hub.GetGameController());
    }

    private void SetGameController(IGameController injection)
    {
        
        this.gameController = injection;
        this.gameController.OnSongStart += OnSongStart;
    }

    public float GetDeltaTime()
    {
        return mDeltaTime;
    }

    public float GetTime()
    {
        return mTime;
    }

    public void RestartTime() 
    {
        mTime = 0;
        mDeltaTime = 0;
    }

    private void OnSongStart() 
    {
        playing = true;
    }

    
}
