using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalReciever : MonoBehaviour
{
    public System.Action<bool> onRecieveSignal;
    public void RecieveSignal(bool signal){
        onRecieveSignal?.Invoke(signal);
    }
}
