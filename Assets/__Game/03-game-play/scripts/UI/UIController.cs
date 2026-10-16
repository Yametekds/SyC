using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour, IGameHubDependecy 
{
    public float stepColor;
    public float maxStep;
    public Image panelFinish;
    public Text textPointsFinish;
    public Image panelOpacate;
    public Text textPoints;
    public GameObject panelLoadResource;

    private ILevelLoaderController levelLoader;
    private IGameController gameController;

   
    private void OnDestroy()
    {
        this.gameController.OnTotalPointsUp -= GoodAwserColor;
        this.gameController.OnTotalPointsDown -= BadAwserColor;
        this.gameController.OnSongFinish -= OnSongFinish;
        this.levelLoader.NotifyOnLoadComplete -= OnLevelLoadCompleted;
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

    public void SetGameController(IGameController injection)
    {
        this.gameController = injection;
        this.gameController.OnTotalPointsUp += GoodAwserColor;
        this.gameController.OnTotalPointsDown += BadAwserColor;
        this.gameController.OnSongFinish += OnSongFinish;
    }

    public void SetLevelLoadController(ILevelLoaderController injection)
    {
        this.levelLoader = injection;
        this.levelLoader.NotifyOnLoadComplete += OnLevelLoadCompleted;
    }

    private void OnLevelLoadCompleted()
    {
        this.panelLoadResource.SetActive(false);
    }

    private void BadAwserColor(int points)
    {
        textPoints.text = points + "";

        Color currentColor = panelOpacate.color;
        float calc = currentColor.a + (stepColor * Time.deltaTime);

        float newValueTransperency = calc >= maxStep ? maxStep : calc;
        currentColor.a = newValueTransperency;
        panelOpacate.color = currentColor;
    }

    private void GoodAwserColor(int points)
    {
        textPoints.text = points + "";
        
        Color currentColor = panelOpacate.color;
        float calc = currentColor.a - (stepColor * Time.deltaTime);

        float newValueTransperency = calc < 0 ? 0 : calc;
        currentColor.a = newValueTransperency;
        panelOpacate.color = currentColor;
    }

    private void OnSongFinish(int points)
    {
        textPointsFinish.text = points + " Puntos";
        panelFinish.gameObject.SetActive(true);
    }

    
}
