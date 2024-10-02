using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendDataPhP : MonoBehaviour
{
    void OnEnable()
    {
        Simulator.OnNewPlayer += NewPlayerAction;
        
    }

    private void NewPlayerAction(string arg1, string arg2, int arg3, float arg4, DateTime arg5)
    {
        Debug.Log("Hola");

        CallbackEvents.OnAddPlayerCallback?.Invoke(99);
    }
}
