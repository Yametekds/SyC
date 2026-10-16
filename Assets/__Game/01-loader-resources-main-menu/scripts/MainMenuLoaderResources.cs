using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MainMenuLoaderResources : MonoBehaviour
{

    public UnityEvent<float, float> OnLoadUpdate;
    public UnityEvent OnFinish;
    public static string ResourcesPath;
    public const string folderName = "levels";
    public const string levelNamePrefix = "level-";
    public const string jsonFileName = "level";
    public const string previewSongFileName = "preview";
    public const string previewBackgroundFileName = "background-preview";

    private IDictionary<string,MainMenuSong> menuSongs;

    private float progress = 0;
    private float total = 0;
    private bool running = false;

    void Awake()
    {
        menuSongs = new Dictionary<string,MainMenuSong>();
        ResourcesPath = Path.Combine( folderName, levelNamePrefix);
        StartCoroutine(LoadResources());
    }

    private void Update()
    {
        if(!running)
        {
            return;
        }
        OnLoadUpdate?.Invoke(progress, total);
    }

    private IEnumerator LoadResources()
    {
        int folderIndex = 0;
        bool search = true;
        this.running = true;

        do
        {
            this.total = 3.0f;
            this.progress = 0;

            string levelFolder = ResourcesPath + "" + (folderIndex);
            string jsonPath = Path.Combine(levelFolder, jsonFileName);
            string previewSongPath = Path.Combine(levelFolder, previewSongFileName);
            string previewBackgroundPath = Path.Combine(levelFolder, previewBackgroundFileName);

            ResourceRequest loadJsonAsyncTask = null;
            ResourceRequest loadPreviewSongAsyncTask = null;
            ResourceRequest loadPreviewBackgourndAsyncTask = null;
            yield return loadJsonAsyncTask = Resources.LoadAsync<TextAsset>(jsonPath);
            yield return loadPreviewSongAsyncTask = Resources.LoadAsync<AudioClip>(previewSongPath);
            yield return loadPreviewBackgourndAsyncTask = Resources.LoadAsync<Sprite>(previewBackgroundPath);
            while (!loadJsonAsyncTask.isDone || !loadPreviewSongAsyncTask.isDone || !loadPreviewBackgourndAsyncTask.isDone)
            {
                yield return this.progress = loadJsonAsyncTask.progress + loadPreviewSongAsyncTask.progress;
            }
            
            TextAsset json = loadJsonAsyncTask.asset as TextAsset;
            AudioClip clip = loadPreviewSongAsyncTask.asset as AudioClip;
            Sprite background = loadPreviewBackgourndAsyncTask.asset as Sprite;
            
            search = json != null && clip != null && background != null;
            if (search)
            {

                yield return folderIndex++;
                MainMenuSong mainMenuSong = MenuSongFactory.getInstance(json, clip);
                mainMenuSong.SetBackground(background);
                menuSongs.Add(levelFolder, mainMenuSong);
                Resources.UnloadAsset(loadJsonAsyncTask.asset);

            }

        } while (search);

        ResourcesContainer.SetMenuSongs(menuSongs);
        yield return this.progress = this.total;
        this.running = false;
        OnFinish?.Invoke();
    }

    
}
