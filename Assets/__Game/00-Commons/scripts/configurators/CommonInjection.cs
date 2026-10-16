using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CommonInjection<Injection,Dependecy> : MonoBehaviour
    where Injection : IInjection
    where Dependecy : IDependency<Injection>
{
    public Component component;
    private Injection injection;
    
    public Injection GetInjection()
    {
        return injection;
    }

    void OnValidate()
    {
        var con = component.GetComponent<Injection>();
        if (!(con is Injection))
        {
            component = null;
        }
            
    }
    
    protected virtual void Awake()
    {
        if (component != null) 
        {
            var con = component.GetComponent<Injection>();
            injection = con;
        }
            
        var dependencies = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<Dependecy>();
        foreach(var d in dependencies) 
        {
            d.SetInjection(injection);
        }
    }

    
}
