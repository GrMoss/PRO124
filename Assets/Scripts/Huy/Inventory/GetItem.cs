using UnityEngine;
using Photon.Pun;

public class GetItem : MonoBehaviourPun
{
    public int itemID; // ID của vật phẩm
    public int quantity; // Số lượng vật phẩm

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && photonView.IsMine)
        {
            // Lấy component Inventory_Manager của người chơi
            Inventory_Manager inventoryManager = collision.GetComponent<Inventory_Manager>();

            if (inventoryManager != null)
            {
                // Thêm vật phẩm vào Inventory
                inventoryManager.AddItemInList(itemID, quantity);

                // Đồng bộ hóa thông tin vật phẩm giữa các người chơi
                photonView.RPC("DestroyItem", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    private void DestroyItem()
    {
        // Hủy đối tượng vật phẩm sau khi nhặt
        PhotonNetwork.Destroy(gameObject);
    }
}
