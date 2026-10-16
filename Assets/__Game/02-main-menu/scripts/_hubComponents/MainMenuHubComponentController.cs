using System;

public class MainMenuHubComponentController : CommonHubComponentController, IMainMenuHubComponentController,
     IMainKeyboardDependecy, ISongSelectorDependency, IBeatControllerDependency
{

    public event Action<IMainMenuHubComponentController> OnLoadComplete;
    private static MainMenuHubComponentController INSTANCE;

    private sealed class KeyArrayPosition
    {
        public const int KEYBOARD_CONTROLLER = 0;
        public const int SONG_SELECTOR_CONTROLLER = 1;
        public const int BEAT_CONTROLLER = 2;
    }
   
    private IMainMenuKeyboardController buttonController;
    private ISongSelector songSelector;
    private IBeatController beatController;

    private void Awake()
    {
        if (INSTANCE != null)
        {
            Destroy(this);
        }
        else
        {
            INSTANCE = this;
        }
    }

    protected override void OnLoadCompleted()
    {
        this.OnLoadComplete?.Invoke(this);
    }

    protected override void InitialSetupComponents()
    {
        SetComponent(KeyArrayPosition.KEYBOARD_CONTROLLER, null);
        SetComponent(KeyArrayPosition.SONG_SELECTOR_CONTROLLER, null);
        SetComponent(KeyArrayPosition.BEAT_CONTROLLER, null);
    }

    public IMainMenuKeyboardController GetKeyBoardController()
    {
        return this.buttonController;
    }

    public ISongSelector GetSongSelectorController()
    {
        return this.songSelector;
    }

    public IBeatController GetBeatController()
    {
        return this.beatController;
    }

    public void SetInjection(IMainMenuKeyboardController injection)
    {
        this.buttonController = injection;
        SetComponent(KeyArrayPosition.KEYBOARD_CONTROLLER, injection);
    }

    public void SetInjection(ISongSelector injection)
    {
        this.songSelector = injection;
        SetComponent(KeyArrayPosition.SONG_SELECTOR_CONTROLLER, injection);
    }

    public void SetInjection(IBeatController injection)
    {
        this.beatController = injection;
        SetComponent(KeyArrayPosition.BEAT_CONTROLLER, injection);
    }

    
}
