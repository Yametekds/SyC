using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class LoadingBarController : MonoBehaviour
{
    [SerializeField]
    private Scrollbar scrollbar;

    public abstract void Subscriptions();

    protected void UpdateScroll(int count, int total)
    {
        scrollbar.value = 0;
        scrollbar.size = total;
        scrollbar.value = count;
    }

    protected void UpdateScroll(float count, float total)
    {
        scrollbar.value = 0;
        scrollbar.size = total;
        scrollbar.value = count;
    }
}
