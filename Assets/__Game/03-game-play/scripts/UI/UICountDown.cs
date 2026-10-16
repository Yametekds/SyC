using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICountDown : MonoBehaviour, IGameHubDependecy
{
    private IGameController gameController;
    public GameObject panelCountDown;
    public Text textPointsFinish;
    

    private void OnDestroy()
    {
        this.gameController.OnSongStart -= OnSongStart;
        this.gameController.OnSongCountDown -= OnCountDown;
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

    public void SetGameController(IGameController injection)
    {
        this.gameController = injection;
        this.gameController.OnSongStart += OnSongStart;
        this.gameController.OnSongCountDown += OnCountDown;
    }

    private void OnSongStart() 
    {
        if (panelCountDown != null) 
        {
            panelCountDown.SetActive(false);
        }
    }

    private void OnCountDown(int count)
    {
        if (panelCountDown != null)
        {
            panelCountDown.SetActive(true);
        }
        
        textPointsFinish.text = count.ToString();
    }
}
