using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(2)]
public class SongSelectorInjection : CommonInjection<ISongSelector,ISongSelectorDependency>
{
    protected override void Awake()
    {
        base.Awake();
        GetInjection().Play(ResourcesContainer.GetSelectedSong());
    }
}
