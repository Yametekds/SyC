using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BackgroundLayer 
{
    private readonly Sprite positive;
    private readonly Sprite negative;

    public Sprite Positive { get { return positive; } }
    public Sprite Negative { get { return negative; } }

    public Sprite Get(int k)
    {
        return k == 0 ? positive : negative;
    }

    public BackgroundLayer(Sprite positive, Sprite negative)
    {
        this.positive = positive;
        this.negative = negative;
    }

   
}
