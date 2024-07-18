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
    private Inventory_Bar inventory_Bar;
    private CookingController cookingController;
    private ItemSlot itemSlot;

    PlayerAnimatorController aniController;

    private bool selectingItem; // Biến để kiểm tra xem có đang trong quá trình chọn item từ inventory_Bar hay không

    PlayerAudio sound;

    private void Start()
    {
        itemSlot = FindObjectOfType<ItemSlot>();
        inventory_Bar = FindObjectOfType<Inventory_Bar>();
        aniController = GetComponentInParent<PlayerAnimatorController>();
        cookingController = FindObjectOfType<CookingController>();
        indexChooseFood = 0;

        sound = GetComponentInParent<PlayerAudio>();

        // Tìm Inventory_Manager trên đối tượng cha trước
        inventory_Manager = GetComponentInParent<Inventory_Manager>();

        // Nếu không tìm thấy, dùng FindObjectOfType để tìm trong toàn bộ scene
        if (inventory_Manager == null)
        {
            inventory_Manager = FindObjectOfType<Inventory_Manager>();
        }

        if (photonView.IsMine)
        {
            mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            view = GetComponentInParent<PhotonView>();
            spriteFood = GetComponentInChildren<SpriteRenderer>();
        }
    }

    private void Update()
    {
        if (food[indexChooseFood].GetComponent<SpriteRenderer>() != null)
        {
            spriteFood.sprite = food[indexChooseFood].GetComponent<SpriteRenderer>().sprite;
        }

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

        indexChooseFood = inventory_Bar.GetIndexShooting();

        if (inventory_Manager.GetQuantityItem(indexChooseFood) <= 0)
        {
            indexChooseFood = 0;
        }

        // Kiểm tra nếu đang trong quá trình chọn item từ inventory_Bar thì không cho phép bắn đạn
        if (!selectingItem && inventory_Manager.GetQuantityItem(indexChooseFood) > 0)
        {
            if (Input.GetMouseButton(0) && canFire && food != null)
            {
                aniController.AttackAnimation();
                sound.PlayerAttack();

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
                aniController.EatAnimation();
                sound.isEating = true;

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

    // Phương thức này sẽ được gọi từ ItemSlot khi bắt đầu hoặc kết thúc việc chọn item
    public void SetSelectingItem(bool isSelecting)
    {
        selectingItem = isSelecting;
    }
}