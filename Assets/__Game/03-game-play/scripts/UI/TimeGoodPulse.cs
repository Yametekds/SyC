using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGoodPulse : MonoBehaviour, IGameHubDependecy
{
    public Sprite[] sprites;
    public Sprite[] spritesInvertido;
    public UnityEngine.UI.Image imageRender;
    public GameObject mGameObject;
    public float duration;
    private float time;

    private IGameController gameController;
    private bool show = false;


    
    void Update()
    {
        if(show && Time.timeSinceLevelLoad > time )
        {
            show = false;
            mGameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        this.gameController.OnGoodNote -= OnGoodNote;
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

    private void SetGameController(IGameController gameController)
    {
        this.gameController = gameController;
        this.gameController.OnGoodNote += OnGoodNote;
    }

    private void OnGoodNote(NoteHolder noteHolder)
    {
        GoodMessage(noteHolder.KeyboardButton != 0);
    }

    private void GoodMessage(bool invertido)
    {
        
        show = true;
        int index = Random.Range(0, sprites.Length);
        
        time = Time.timeSinceLevelLoad + duration;
        

        if (invertido)
        {
            imageRender.sprite = spritesInvertido[index];
            
        }
        else
        {
            imageRender.sprite = sprites[index];
        }
        mGameObject.SetActive(true);
    }

    
}
