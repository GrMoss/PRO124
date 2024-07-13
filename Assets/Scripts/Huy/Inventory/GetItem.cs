using UnityEngine;
using Photon.Pun;

public class GetItem : MonoBehaviourPun
{
    public int[] itemID;
    public int quantity;
    private Inventory_Manager inventoryManager; // Tham chiếu đến Inventory_Manager

    private void Start()
    {
        if (photonView.IsMine)
        {
            // Tìm Inventory_Manager trên đối tượng cha trước
            inventoryManager = GetComponentInParent<Inventory_Manager>();

            // Nếu không tìm thấy, dùng FindObjectOfType để tìm trong toàn bộ scene
            if (inventoryManager == null)
            {
                inventoryManager = FindObjectOfType<Inventory_Manager>();
            }

            if (inventoryManager != null)
            {
                Debug.Log("Đã gán Inventory_Manager cho GetItem.");
            }
            else
            {
                Debug.LogError("Không tìm thấy Inventory_Manager trên đối tượng này hoặc trong scene.");
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
                    inventoryManager.AddItemInList(itemID[i], quantity);
                    PhotonNetwork.Destroy(collision.gameObject); // Xóa đối tượng va chạm
                    break; // Thoát khỏi vòng lặp sau khi thêm vật phẩm
                }
            }
        }
    }

}
