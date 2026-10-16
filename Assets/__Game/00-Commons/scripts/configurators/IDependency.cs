using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDependency<injectionKind> where injectionKind : IInjection
{
    void SetInjection(injectionKind injection);
}