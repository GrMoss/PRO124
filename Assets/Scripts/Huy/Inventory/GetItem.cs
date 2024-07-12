using UnityEngine;
using Photon.Pun;

public class GetItem : MonoBehaviourPun
{
    public int[] itemID;
    public int[] quantity;
    private Inventory_Manager inventoryManager; // Tham chiếu đến Inventory_Manager

    private PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        inventoryManager = GetComponent<Inventory_Manager>();

        if (inventoryManager != null)
        {
            Debug.Log("Đã gán Inventory_Manager cho GetItem.");
        }
        else
        {
            Debug.LogError("Không tìm thấy Inventory_Manager trên đối tượng này.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (view.IsMine)
        {
            for (int i = 0; i < itemID.Length; i++)
            {
                if (collision.CompareTag(i.ToString()))
                {
                    // Thêm vật phẩm vào túi của người chơi hiện tại
                    inventoryManager.AddItemInList(itemID[i], quantity[i]);
                    PhotonNetwork.Destroy(collision.gameObject); // Xóa đối tượng va chạm
                    break; // Thoát khỏi vòng lặp sau khi thêm vật phẩm
                }
            }
        }
    }
}
