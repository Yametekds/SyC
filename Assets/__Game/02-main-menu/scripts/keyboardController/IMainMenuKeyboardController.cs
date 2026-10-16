using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMainMenuKeyboardController : IInjection
{
    public enum GameKeys
    {
        leftButton,
        rigthButton,
        startSongButton,
    };

    public event Action<IMainMenuKeyboardController.GameKeys> OnButtonPress;

}
