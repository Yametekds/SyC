using System;
using System.Collections;
using UnityEngine;


public class GameController : MonoBehaviour, IGameController, IGameHubDependecy
{

    public event Action<int> OnSongCountDown;
    public event Action OnSongStart;
    public event Action<NoteHolder> OnGoodNote;
    public event Action<int> OnTotalPointsUp;
    public event Action<int> OnTotalPointsDown;
    public event Action<int> OnSongFinish;

    public int startSongTime = 3;
    public float timeAfterFinish = 3.0f;
    public string upDectectNoteInput;
    public string downDectectNoteInput;

    public DetectNoteController upDectecNote;
    public DetectNoteController downDectecNote;

    /**todo extract to another class*/
    public Animator noirAnimator;
    public Animator solAnimator;

    
    private static GameController INSTANCE;
    private ILevelLoaderController levelLoader;
    
    
    
    
    private int auxStartTime;

    private int gamePoints = 0;

    private void Awake()
    {
        if (INSTANCE != null)
        {
            Destroy(this);
        }
        else
        {
            INSTANCE = this;
            this.auxStartTime = this.startSongTime;
            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
            upDectecNote.InputName = upDectectNoteInput;
            downDectecNote.InputName = downDectectNoteInput;
            upDectecNote.OnFailNote += ReducePoints;
            downDectecNote.OnFailNote += ReducePoints;
            upDectecNote.OnGoodNote += GoodNotePress;
            downDectecNote.OnGoodNote += GoodNotePress;
        }
    }



    private void OnDestroy()
    {
        this.levelLoader.NotifyOnLoadComplete -= StartCountDown;
        upDectecNote.OnFailNote -= ReducePoints;
        downDectecNote.OnFailNote -= ReducePoints;
        upDectecNote.OnGoodNote -= GoodNotePress;
        downDectecNote.OnGoodNote -= GoodNotePress;
    }

    public void SetInjection(IGameHubComponentController injection)
    {

        injection.OnLoadComplete += this.OnHubCompleteLoad;
    }

    private void OnHubCompleteLoad(IGameHubComponentController hub)
    {
        hub.OnLoadComplete -= this.OnHubCompleteLoad;
        SetLevelLoadController(hub.GetLevelLoaderController());
    }

    private void SetLevelLoadController(ILevelLoaderController injection)
    {
        this.levelLoader = injection;
        this.levelLoader.NotifyOnLoadComplete += StartCountDown;
    }

    public void FinishSong() 
    {
        StartCoroutine(FinishSongCoroutine());
    }

    public void StartCountDown() 
    {
        StartCoroutine(CountDownToStart());
    }
    

    private void ReducePoints() 
    {
       
        gamePoints--;
        if (gamePoints < 0) 
        {
            gamePoints = 0;
        }
        OnTotalPointsDown?.Invoke(gamePoints);
        
    }

    private void GoodNotePress(NoteHolder note)
    {
        gamePoints += note.RangeZonePoints();
        OnTotalPointsUp?.Invoke(gamePoints);
        OnGoodNote?.Invoke(note);
        

        int k = note.KeyboardButton;
        noirAnimator.SetLayerWeight(1, k);
        solAnimator.SetLayerWeight(1, k);
    }


    

    private IEnumerator CountDownToStart() 
    {    
        WaitForSeconds waitHalfSecond = new WaitForSeconds(0.5f);

        while(auxStartTime > 0)
        {
            yield return waitHalfSecond;
            OnSongCountDown?.Invoke(auxStartTime);
            auxStartTime--;
            yield return waitHalfSecond;

        }
        OnSongCountDown?.Invoke(0);
        OnSongStart?.Invoke();
    }

    private IEnumerator FinishSongCoroutine() 
    {
        WaitForSeconds waitTime = new WaitForSeconds(timeAfterFinish);
        yield return waitTime;
        OnSongFinish?.Invoke(gamePoints);
    }

    
}
