using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DestroyItem : MonoBehaviourPun
{
    public delegate void ItemDestroyedHandler(GameObject item);
    public event ItemDestroyedHandler OnItemDestroyed;

    private void OnDestroy()
    {
        OnItemDestroyed?.Invoke(this.gameObject);
    }

}
