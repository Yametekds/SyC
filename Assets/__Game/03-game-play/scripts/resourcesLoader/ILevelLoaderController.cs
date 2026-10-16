using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ILevelLoaderController : IInjection
{
    public event Action<int, int> OnLoadUpdate;
    public event Action<LevelModel> OnLoadComplete;
    public event Action NotifyOnLoadComplete;
    public event Action OnUnLoadComplete;
    public void LoadResources();
}
