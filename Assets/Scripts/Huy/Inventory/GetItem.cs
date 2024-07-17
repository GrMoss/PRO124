using UnityEngine;
using Photon.Pun;

public class GetItem : MonoBehaviourPun
{
    public int[] itemID;
    public int quantity;
 
    private Inventory_Manager inventory_Manager;
    private Inventory_Bar inventory_Bar;

    private void Start()
    {
        // Tìm Inventory_Manager trên đối tượng cha trước
        inventory_Manager = GetComponentInParent<Inventory_Manager>();
        inventory_Bar = FindObjectOfType<Inventory_Bar>();
        if (photonView.IsMine)
        {
            if (inventory_Manager != null)
            {
                Debug.Log("Đã gán Inventory_Manager cho ItemPrefab.");
            }
            else
            {
                Debug.LogError("Không tìm thấy Inventory_Manager trên đối tượng này hoặc trong scene.");
            }

            if (inventory_Bar != null)
            {
                Debug.Log("Đã gán inventory_UI cho ItemPrefab.");
            }
            else
            {
                Debug.LogError("Không tìm thấy inventory_UI trên đối tượng này hoặc trong scene.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (photonView.IsMine)
        {
            for (int i = 0; i < itemID.Length; i++)
            {
                if (collision.CompareTag(i.ToString()))
                {
                    // Thêm vật phẩm vào túi của người chơi hiện tại
                    inventory_Manager.AddItemInList(itemID[i], quantity);
                    PhotonNetwork.Destroy(collision.gameObject); // Xóa đối tượng va chạm
                    break; // Thoát khỏi vòng lặp sau khi thêm vật phẩm
                }
            }
        }
    }

}
