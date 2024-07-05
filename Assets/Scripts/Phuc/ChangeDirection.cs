using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDirection : MonoBehaviour
{
    public GameObject player;
    private PhotonView view;

    private void Start()
    {
        view = GetComponentInParent<PhotonView>();
    }

    private void Update()
    {
        if (view.IsMine)
        {
            if (transform.position.x <= player.transform.position.x)
            {
                transform.localScale = new Vector3(transform.localScale.x, -Mathf.Abs(transform.localScale.y), transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), transform.localScale.z);
            }
        }
    }
}
