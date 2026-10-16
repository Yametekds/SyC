using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuKeyboardController : MonoBehaviour, IMainMenuKeyboardController
{
    

    public event Action<IMainMenuKeyboardController.GameKeys> OnButtonPress;
    private bool axisButtonIsPress;
    private int axisInput;
    private bool startSong;

    private void Awake()
    {
        startSong = false;
        axisButtonIsPress = false;
    }

    void Update()
    {
        
        if(startSong) 
        {
            return;
        }

        axisInput = Mathf.CeilToInt(Input.GetAxisRaw("Horizontal"));
        if (axisInput == 0)
        {
            axisButtonIsPress = false;
        }
        else 
        {
            if (!axisButtonIsPress)
            {
                axisButtonIsPress = true;
                if (axisInput > 0)
                {
                    OnButtonPress?.Invoke(IMainMenuKeyboardController.GameKeys.rigthButton);
                }
                else
                {
                    OnButtonPress?.Invoke(IMainMenuKeyboardController.GameKeys.leftButton);
                }
            }
        }
        
        if(Input.GetButtonDown("Jump"))
        {
            OnButtonPress?.Invoke(IMainMenuKeyboardController.GameKeys.startSongButton);
            startSong = true;
        }
        
        
    }
}
