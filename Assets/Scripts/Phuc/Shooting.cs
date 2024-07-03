using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviourPun
{
    private Camera mainCam;
    private Vector3 mousePos;
    public GameObject food;
    public Transform foodTrans;
    private bool canFire = true;
    private float timer;
    public float timeBetweenFiring;

    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

            Vector3 rotation = mousePos - transform.position;

            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, rotZ);

            if (!canFire)
            {
                timer += Time.deltaTime;
                if (timer > timeBetweenFiring)
                {
                    canFire = true;
                    timer = 0;
                }
            }

            if (Input.GetMouseButton(0) && canFire)
            {
                canFire = false;
                Instantiate(food, foodTrans.position + new Vector3(transform.position.x, transform.position.y,
                    transform.position.z), Quaternion.identity);
            }
        }
    }
}
