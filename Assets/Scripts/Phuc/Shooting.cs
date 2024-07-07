using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public GameObject food;
    public Transform foodTrans;
    private bool canFire = true;
    private float timer;
    public float timeBetweenFiring;
    public GameObject player;
    private PhotonView view;
    private SpriteRenderer spriteFood;

    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        view = GetComponentInParent<PhotonView>();
        spriteFood = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        spriteFood.sprite = food.GetComponent<SpriteRenderer>().sprite;
        if (view.IsMine)
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

            if (Input.GetMouseButton(0) && canFire && food != null)
            {
                canFire = false;
                GameObject foodObject = PhotonNetwork.Instantiate(food.name, foodTrans.position + new Vector3(transform.position.x, transform.position.y,
                    transform.position.z), Quaternion.identity);
                foodObject.GetComponent<Food>().ownerId = view.ViewID;
                Debug.Log($"Food instantiated with ownerId = {view.ViewID}");
            }

            if (Input.GetMouseButton(1) && canFire && food != null)
            {
                canFire = false;
                GameObject foodObject = PhotonNetwork.Instantiate(food.name, foodTrans.position + new Vector3(transform.position.x, transform.position.y,
                    transform.position.z), Quaternion.identity);
                foodObject.GetComponent<Food>().ownerId = 1;
                Debug.Log($"Food instantiated with ownerId = {view.ViewID}");
            }
        }
    }
}
