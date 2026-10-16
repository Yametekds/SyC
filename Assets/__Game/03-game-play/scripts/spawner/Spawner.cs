using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour, IGameHubDependecy
{

    public int initialPoolNotes;
    public GameObject prefab;
    
    public Transform higthStartPosition, lowStartPosition;
    public Transform higthEndPosition, lowEndPosition;
    public AudioClip[] lowTone;
    public AudioClip[] middleTone;
    public AudioClip[] higthTone;

    
    private IGameController gameController;
    private ITimeController timeController;
    private ILevelLoaderController levelLoader;
    private IBeatController beatController;

    private SongWithDetails song;
    private readonly PoolOfObjects notePool = new PoolOfObjects();
    private bool songStart = false;
    private float mTime;
    private float mDistance;
    private IDictionary<int, Action<NoteHolder>> noteSetupMap;
    


    private void Awake()
    {
        InitMapConfig();
        notePool.OnObjectReturn += IsTheEnd;
        Vector3 d1 = higthEndPosition.position - higthStartPosition.position;
        Vector3 d2 = lowEndPosition.position - lowStartPosition.position;
        Vector3 d3 = d1.Equals(d2) ? d1 : (d1.x > d2.x ? d1 : d2);
        mDistance = Mathf.Abs( d3.x);

        initialPoolNotes = initialPoolNotes < 1 ? 10 : initialPoolNotes;
        notePool.Transform = transform;
        notePool.Prefab = prefab;
        Enumerable
            .Range(0, initialPoolNotes)
            .Select(i => notePool.GetObject())
            .ToList()
            .ForEach(n => notePool.ReturnObject(n));

       
    }

    private void OnDestroy()
    {
        notePool.OnObjectReturn -= IsTheEnd;
        gameController.OnSongStart -= OnSongStart;
        levelLoader.OnLoadComplete -= SetLevelModel;
        beatController.OnBeatChange -= SetBeatInfo;
    }

    void Update()
    {
        if (songStart && song != null && !song.IsTheEnd())
        {
            mTime = this.timeController.GetTime(); 
            if ( song.SpawnNote(mTime))
            {
                try
                {
                    DataNote dataNote = song.PopNote();
                    GameObject gameObject = notePool.GetObject();
                    NoteHolder holder = gameObject.GetComponent<NoteHolder>();
                    holder.BindHolder(dataNote);
                    holder.SetupNote();
                    int key = dataNote.key;
                    noteSetupMap[key]?.Invoke(holder);
                    gameObject.transform.SetParent(null);
                    gameObject.SetActive(true);
                }
                catch (Exception e) {
                    Debug.Log(e);
                }
            }
        }
        
    }

    public void SetInjection(IGameHubComponentController injection)
    {
        injection.OnLoadComplete += this.OnHubCompleteLoad;
    }

    private void OnHubCompleteLoad(IGameHubComponentController hub) 
    {
        hub.OnLoadComplete -= this.OnHubCompleteLoad;
        SetLevelLoadController(hub.GetLevelLoaderController());
        SetGameController(hub.GetGameController());
        SetTimeController(hub.GetTimeController());
        SetBeatController(hub.GetBeatController());
    }

    private void SetLevelLoadController(ILevelLoaderController injection)
    {
        this.levelLoader = injection;
        this.levelLoader.OnLoadComplete += SetLevelModel;
    }

    private void SetGameController(IGameController injection)
    {
        this.gameController = injection;
        gameController.OnSongStart += OnSongStart;
    }

    private void SetTimeController(ITimeController injection)
    {
        this.timeController = injection;
        NoteHolder.SetTimeController(injection);
    }
    
    private void SetBeatController(IBeatController injection) 
    {
        this.beatController = injection;
        this.beatController.OnBeatChange += SetBeatInfo;
    }

    private void SetBeatInfo(BeatInfo beatInfo) 
    {
        float translationTime = beatInfo.Bps - ( 2 * this.song.hitOffset);
       
        if (translationTime < 0)
        {
            translationTime *= (-1);
        }
        float speed = mDistance / translationTime;
        if (speed < 0)
        {
            speed *= (-1);
        }
        
        NoteHolder.SetSpeed(speed);
        NoteHolder.SetTranslationTIme(translationTime);
        NoteHolder.SetBeatInfo(beatInfo);
    }

    private void SetLevelModel(LevelModel level)
    {
        this.song = level.GetSongWithDetails();

        higthTone = ToneResolver(level.GetCeremonyArcs()[0].GetHigthGamePlayTone(), higthTone);
        middleTone = ToneResolver(level.GetCeremonyArcs()[0].GetMidGamePlayTone(), middleTone);
        lowTone = ToneResolver(level.GetCeremonyArcs()[0].GetLowGamePlayTone(), lowTone);
    }

    private void OnSongStart() 
    {
        songStart = true;
    }

    private void IsTheEnd()
    {
        if (songStart && song.IsTheEnd())
        {
            songStart = false;
            this.gameController.FinishSong();
        }
    }

    private void InitMapConfig()
    {
        noteSetupMap = new Dictionary<int, Action<NoteHolder>>();
        noteSetupMap.Add(GamePlayKeys.UP_POSITIVE, HightNoteConfig);
        noteSetupMap.Add(GamePlayKeys.DOWN_NEGATIVE, DownNoteConfig);
        noteSetupMap.Add(2, MidleNoteConfig);
    }

    private void HightNoteConfig(NoteHolder note) 
    {
        int noteIndex = UnityEngine.Random.Range(0, higthTone.Length);
        note.gameObject.transform.position = higthStartPosition.position;
        note.initialPosition = higthStartPosition.position;
        note.SetAudioClip(higthTone[noteIndex]);
        note.KeyboardButton = GamePlayKeys.UP_POSITIVE;
    }

    private void DownNoteConfig(NoteHolder note)
    {
        int noteIndex = UnityEngine.Random.Range(0, lowTone.Length);
        note.gameObject.transform.position = lowStartPosition.position;
        note.initialPosition = lowStartPosition.position;
        note.SetAudioClip(lowTone[noteIndex]);
        note.KeyboardButton = GamePlayKeys.DOWN_NEGATIVE;
    }

    private void MidleNoteConfig(NoteHolder note)
    {
        int noteIndex = UnityEngine.Random.Range(0, middleTone.Length);
        int p = UnityEngine.Random.Range(0, 2);
        noteSetupMap[p]?.Invoke(note);
        note.SetAudioClip(middleTone[noteIndex]);
    }

    private AudioClip[] ToneResolver(AudioClip[] newClicks, AudioClip[] oldClicks) 
    {
        if (newClicks != null && newClicks.Length > 0)
        {
            return newClicks;
        }

        return oldClicks;
    }

    
}
