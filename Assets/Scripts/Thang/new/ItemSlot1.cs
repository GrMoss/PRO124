using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot1 : MonoBehaviour, IPointerClickHandler
{
    //========ITEM DATA========//
    public int id;
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;



    [SerializeField]
    private int maxNumberOfItems;

    //========ITEM SLOT========//
    [SerializeField]
    private TMP_Text quantityText;

    [SerializeField]
    private Image itemImage;

    //=========ITEAM DESCRIPTION SLOT===========//
    public Image ItemDescriptionImage;
    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;


    public GameObject Choose;
    public GameObject selectedShader;
    public bool thisItemSelected;
    private InventoryManager1 inventoryManager1;

    //------------------------------------------//
    public ChooseItem chooseItem;

    private void Start()
    {
        inventoryManager1 = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager1>();
    }
    public int AddItem(int id, string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        //Kiểm tra vị trí đã dầy hay chưa
        if (isFull)
            return quantity;
        this.id = id;

        //Update NAME
        this.itemName = itemName;

        //UpdateImage
        this.itemSprite = itemSprite;
        itemImage.sprite = itemSprite;

        //Update Descriptiom
        this.itemDescription = itemDescription;

        //Update QUANTITY
        this.quantity += quantity;
        if (this.quantity >= maxNumberOfItems)
        {
            quantityText.text = maxNumberOfItems.ToString();
            quantityText.enabled = true;
            isFull = true;

            //Trả về LEFTOVERS
            int extraItems = this.quantity - maxNumberOfItems;
            this.quantity = maxNumberOfItems;
            return extraItems;
        }

        // Update QUANTITY TEXT
        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;

        return 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }
    public void OnLeftClick()
    {
        inventoryManager1.DeselectAllSlots();
        selectedShader.SetActive(true);
        thisItemSelected = true;
        ItemDescriptionNameText.text = itemName;
        ItemDescriptionText.text = itemDescription;
        ItemDescriptionImage.sprite = itemSprite;
        if(ItemDescriptionImage.sprite ==null)
        {
            ItemDescriptionImage.sprite = emptySprite;
        }
        chooseItem.ItemID(id,quantity);
    }
    public void OnRightClick()
    {
        Choose.SetActive(true);
    }
}

