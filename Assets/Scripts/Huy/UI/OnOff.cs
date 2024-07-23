using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OnOff : MonoBehaviourPun
{
    public PhotonView health;
    private void Update()
    {
        if (Timer.TimeOver)
        {
            health.RPC("TurnOffHealth", RpcTarget.All);
        }
    }
}
