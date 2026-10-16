using System.Collections.Generic;
using UnityEngine;

public abstract class CommonHubComponentController : MonoBehaviour
{

    private bool start = false;
    private IDictionary<int, IInjection> componentTracking;

    protected abstract void OnLoadCompleted();
    protected abstract void InitialSetupComponents();

    protected void SetComponent(int id, IInjection component)
    {
        InitMapComponents();

        if (componentTracking.ContainsKey(id))
        {
            componentTracking[id] = component;
        }
        else
        {
            componentTracking.Add(id, component);
        }

        VerifyAllSetup();
    }

    private void VerifyAllSetup()
    {
        bool allLoad = true;
        foreach (int key in componentTracking.Keys)
        {
            IInjection component = componentTracking[key];
            allLoad = allLoad && (component != null);
            if (!allLoad)
            {
                break;
            }

        }

        if (!this.start && allLoad)
        {
            this.start = true;
            this.OnLoadCompleted();
        }
    }
    private void InitMapComponents()
    {
        if (componentTracking == null)
        {
            componentTracking = new Dictionary<int, IInjection>();
            InitialSetupComponents();
        }
    }
}
