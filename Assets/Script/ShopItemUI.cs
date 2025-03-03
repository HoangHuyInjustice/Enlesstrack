using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemUI : MonoBehaviour
{
    public List<ShopItemInfo> ShopItemInfos;

    public SlotItemUI SlotItemUI;

    [SerializeField]
    private Transform _containerItem;
    public static ShopItemUI Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        DisplayPlayers();
    }
    private void DisplayPlayers()// hiển thị slot các Item có trong data shopinfoItem
    {
        foreach (var shopInfo in ShopItemInfos)
        {
            SlotItemUI slot = Instantiate(SlotItemUI, _containerItem);
            slot.SetUpUI(shopInfo);
            slot.SetButtonState(shopInfo, this);
        }
    }

    public void SetButtonState(int itemID, bool canBuy)
    {
      
    }


}
[System.Serializable]
public class ShopItemInfo
{
    public string ItemName;
    public string Description;
    public int ItemID;
    public Sprite AvatarItem;
    public int Price;
    public int Amount; 
}
