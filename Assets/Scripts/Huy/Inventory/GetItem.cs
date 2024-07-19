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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (photonView.IsMine)
        {
            for (int i = 0; i < itemID.Length; i++)
            {
                if (collision.gameObject.CompareTag(i.ToString()))
                {
                    // Thêm vật phẩm vào túi của người chơi hiện tại
                    inventory_Manager.AddItemInList(itemID[i], quantity);
                    //inventory_Bar.UpdateInventoryBar();
                    break; // Thoát khỏi vòng lặp sau khi thêm vật phẩm
                }
            }
        }
    }

}
