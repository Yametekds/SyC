using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundControllerGameplay : MonoBehaviour, IGameHubDependecy
{

    public SpriteRenderer backgroundRender;
    
    private Sprite[] spriteBackgroundArray;
    private ILevelLoaderController levelLoader;
    private IGameController gameController;


    private void OnDestroy()
    {
        this.gameController.OnGoodNote -= OnGoodNote;
        this.levelLoader.OnLoadComplete -= OnLoadComplete;
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
    }

    private void SetLevelLoadController(ILevelLoaderController injection)
    {
        this.levelLoader = injection;
        this.levelLoader.OnLoadComplete += OnLoadComplete;
    }

    private void SetGameController(IGameController injection)
    {
        this.gameController = injection;
        this.gameController.OnGoodNote += OnGoodNote;
    }

    private void OnLoadComplete(LevelModel obj)
    {
        spriteBackgroundArray = new Sprite[] {
            obj.GetCeremonyArcs()[0].GetBackground()[0].Get(0),
            obj.GetCeremonyArcs()[0].GetBackground()[0].Get(1)
        };

        this.backgroundRender.sprite = spriteBackgroundArray[0];
    }

    private void OnGoodNote(NoteHolder noteHolder)
    {
        int k = noteHolder.KeyboardButton;
        backgroundRender.sprite = spriteBackgroundArray[k];
    }

    
}
