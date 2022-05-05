using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalSender : MonoBehaviour
{
    [SerializeField] SignalReciever[] targets;
    public void SetSignal(bool value){
        foreach(SignalReciever target in targets){
            target.RecieveSignal(value);
        }
    }
}
