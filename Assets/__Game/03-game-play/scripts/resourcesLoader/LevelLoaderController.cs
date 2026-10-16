using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelLoaderController : MonoBehaviour, ILevelLoaderController
{
    public event Action<int, int> OnLoadUpdate;
    public event Action<LevelModel> OnLoadComplete;
    public event Action NotifyOnLoadComplete;
    public event Action OnUnLoadComplete;

    private string levelPath;
    private IList<UnityEngine.Object> resoursesLoaded;
    private const string JSON = "level";
    private const string BASE_SONG = "base-song";
    private const string ACT_PREFIX = "act-";
    private const string BACKGROUND_POSITIVE = "positive/background";
    private const string BACKGROUND_NEGATIVE = "negative/background";
    private const string NOTES_HIGTH = "notes/higth";
    private const string NOTES_MID = "notes/middle";
    private const string NOTES_LOW = "notes/low";

    private int totalItems = 3;
    private int count = 0;
    private bool running = false;

    public void LoadResources() 
    {
        StartCoroutine(LoadResourcesCoroutine());
    }

    public void UnLoadResources()
    {
        StartCoroutine(UnloadResourcesCoroutine());
    }

    private void Awake()
    {
        resoursesLoaded = new List<UnityEngine.Object>();
        levelPath = ResourcesContainer.GetSelectedSongPath();
    }
    /*
    private void Update()
    {
        if(!running) 
        {
            return;
        }

        OnLoadUpdate?.Invoke(count, totalItems);
    }
    */
    private IEnumerator LoadResourcesCoroutine()
    {
        LevelModel levelModel = new LevelModel();
        
        string jsonPath = Path.Combine(levelPath,JSON);
        string songPath = Path.Combine(levelPath,BASE_SONG);
        ResourceRequest loadJsonAsyncTask = null;
        ResourceRequest loadSongAsyncTask = null;

        this.running = true;
        yield return loadJsonAsyncTask = Resources.LoadAsync<TextAsset>(jsonPath);
        yield return loadSongAsyncTask = Resources.LoadAsync<AudioClip>(songPath);

        while (!loadJsonAsyncTask.isDone || !loadSongAsyncTask.isDone)
        {
            yield return loadJsonAsyncTask.progress + loadSongAsyncTask.progress;
            if ( count == 0 && (loadJsonAsyncTask.isDone || loadSongAsyncTask.isDone)) 
            {
                yield return count = 1;
                
            }
            
        }
        
        yield return count;
        TextAsset json = loadJsonAsyncTask.asset as TextAsset;
        AudioClip clip = loadSongAsyncTask.asset as AudioClip;
        resoursesLoaded.Add(json);
        resoursesLoaded.Add(clip);
        
        levelModel.SetBaseClip(clip);
        levelModel.SetSongWithDetails(json);
        yield return count = 2;
        bool searchActs = true;
        int actIndex = 0;
        IList<CeremonyArc> ceremonyArcs = new List<CeremonyArc>();
        while (searchActs)
        {
            string actPath = ACT_PREFIX + actIndex;
            actPath = Path.Combine(levelPath, actPath);
            string postiveBagroundPath = Path.Combine(actPath, BACKGROUND_POSITIVE);
            string negativeBagroundPath = Path.Combine(actPath, BACKGROUND_NEGATIVE);
            string lowTonePath = Path.Combine(actPath, NOTES_LOW);
            string midTonePath = Path.Combine(actPath, NOTES_MID);
            string higthTonePath = Path.Combine(actPath, NOTES_HIGTH);

            Sprite[] positveBackgrounds;
            Sprite[] negativeBackgrounds;
            AudioClip[] lowTone;
            AudioClip[] middleTone;
            AudioClip[] higthTone;

            yield return positveBackgrounds = Resources.LoadAll<Sprite>(postiveBagroundPath);
            yield return negativeBackgrounds = Resources.LoadAll<Sprite>(negativeBagroundPath);

            yield return lowTone = Resources.LoadAll<AudioClip>(lowTonePath);
            yield return middleTone = Resources.LoadAll<AudioClip>(midTonePath);
            yield return higthTone = Resources.LoadAll<AudioClip>(higthTonePath);
            

            searchActs = positveBackgrounds != null && negativeBackgrounds != null;
            searchActs = searchActs && (positveBackgrounds.Length > 0 && negativeBackgrounds.Length > 0);
            searchActs = searchActs && (positveBackgrounds.Length == negativeBackgrounds.Length);
            
            if (searchActs) 
            {
                actIndex++;
                CeremonyArc arc = new CeremonyArc();
                IList<BackgroundLayer> background = new List<BackgroundLayer>();
                for(int i = 0; i < positveBackgrounds.Length; i++)
                {
                    BackgroundLayer layer = new BackgroundLayer(positveBackgrounds[i], negativeBackgrounds[i]);
                    background.Add(layer);
                    resoursesLoaded.Add(positveBackgrounds[i]);
                    resoursesLoaded.Add(negativeBackgrounds[i]);
                    yield return layer;
                }
                arc.SetLowGamePlayTone(lowTone);
                arc.SetMidGamePlayTone(middleTone);
                arc.SetHigthGamePlayTone(higthTone);
                arc.SetBackgound(background);
                ceremonyArcs.Add(arc);
            }
        }
        levelModel.SetCeremonyArcs(ceremonyArcs);

        this.running = false;
        NotifyOnLoadComplete?.Invoke();
        OnLoadComplete?.Invoke(levelModel);
    }

    private IEnumerator UnloadResourcesCoroutine() 
    {
        int total = resoursesLoaded.Count;
        for (int i = 0; i < total; i++) 
        {
            yield return i;
            Debug.Log("unload " + i + "/" + total);
            OnLoadUpdate?.Invoke(i, total);
            Resources.UnloadAsset(resoursesLoaded[i]);
        }
        resoursesLoaded.Clear();
        OnUnLoadComplete?.Invoke();
    }
}
