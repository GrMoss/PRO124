using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviourPun
{
    [Header("index Choose Start")]
    public int indexChooseFood;

    [Header("Item object")]
    public GameObject[] food;


    private Camera mainCam;
    private Vector3 mousePos;
    public Transform foodTrans;
    private bool canFire = true;
    private float timer;
    public float timeBetweenFiring;
    public GameObject player;
    private PhotonView view;
    private SpriteRenderer spriteFood;
    public float directionY;

    private Inventory_Manager inventory_Manager; // Tham chiếu đến Inventory_Manager

    private void Start()
    {
        indexChooseFood = 0;
        // Tìm Inventory_Manager trên đối tượng cha trước
        inventory_Manager = GetComponentInParent<Inventory_Manager>();

        // Nếu không tìm thấy, dùng FindObjectOfType để tìm trong toàn bộ scene
        if (inventory_Manager == null)
        {
            inventory_Manager = FindObjectOfType<Inventory_Manager>();
        }

        if (photonView.IsMine)
        {
            if (inventory_Manager != null)
            {
                Debug.Log("Đã gán Inventory_Manager cho GetItem.");
            }
            else
            {
                Debug.LogError("Không tìm thấy Inventory_Manager trên đối tượng này hoặc trong scene.");
            }
        }

        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        view = GetComponentInParent<PhotonView>();
        spriteFood = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {


        if (food[indexChooseFood].GetComponent<SpriteRenderer>() != null)
        {
            spriteFood.sprite = food[indexChooseFood].GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            spriteFood.sprite = null;
        }

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

            if (inventory_Manager.GetQuantityItem(indexChooseFood) > 0)
            {

                if (Input.GetMouseButton(0) && canFire && food != null)
                {
                    canFire = false;
                    GameObject foodObject = PhotonNetwork.Instantiate(food[indexChooseFood].name, foodTrans.position + new Vector3(transform.position.x, transform.position.y,
                        transform.position.z), Quaternion.identity);

                    inventory_Manager.QuitItemInList(indexChooseFood, 1);

                    foodObject.transform.localScale = new Vector3(
                    foodObject.transform.localScale.x,
                    foodObject.transform.localScale.y * directionY,
                    foodObject.transform.localScale.z);
                    foodObject.GetComponent<Food>().ownerId = view.ViewID;
                    Debug.Log($"Food instantiated with ownerId = {view.ViewID}");
                }

                if (Input.GetMouseButton(1) && canFire && food != null)
                {
                    canFire = false;
                    GameObject foodObject = PhotonNetwork.Instantiate(food[indexChooseFood].name, foodTrans.position + new Vector3(transform.position.x, transform.position.y,
                        transform.position.z), Quaternion.identity);

                    inventory_Manager.QuitItemInList(indexChooseFood, 1);

                    foodObject.transform.localScale = new Vector3(
                    foodObject.transform.localScale.x,
                    foodObject.transform.localScale.y * directionY,
                    foodObject.transform.localScale.z);
                    foodObject.GetComponent<Food>().ownerId = 1;
                    Debug.Log($"Food instantiated with ownerId = {view.ViewID}");
                }
            }
        }
    }
}
