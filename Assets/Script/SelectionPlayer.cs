using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionPlayer : MonoBehaviour
{
    [SerializeField]
    private List<ShopInfoPlayer> _shopInfos;

    [SerializeField]
    private SlotPlayerUI slotPlayerUI;

    [SerializeField]
    private Transform _containerShop;

    public int CurrentPlayer;

    private List<SlotPlayerUI> _slotUIs = new List<SlotPlayerUI>();

    public static SelectionPlayer Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadData();
        DisplayPlayers();
        EnsureDefaultPlayerBought();
        SelectPlayer(0);
    }

    private void DisplayPlayers()// hiển thị slot các nhân vật có trong data shopinfo
    {
        foreach (var shopInfo in _shopInfos)
        {
            SlotPlayerUI slot = Instantiate(slotPlayerUI, _containerShop);
            slot.SetUpUI(shopInfo);
            slot.SetButtonState(shopInfo, this);
            _slotUIs.Add(slot);
        }
    }

    public void SaveData()                 // Lưu trữ shop xem những nhân vật nào đã mua
    {
        foreach (var shopInfo in _shopInfos)
        {
            PlayerPrefs.SetInt($"Player_{shopInfo.PlayerID}_Bought", shopInfo.Bought ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    public void LoadData()                 // khi bắt đầu game thì tải lại những danh sách player đã mua 
    {
        foreach (var shopInfo in _shopInfos)
        {
            shopInfo.Bought = PlayerPrefs.GetInt($"Player_{shopInfo.PlayerID}_Bought", 0) == 1;
        }
    }
    private void EnsureDefaultPlayerBought()// Nhân vật đầu tiên lúc nào cũng đã mua và chơi đc 
    {
        foreach (var shopInfo in _shopInfos)
        {
            if (shopInfo.PlayerID == 0)
            {
                shopInfo.Bought = true;
                foreach (var slot in _slotUIs)
                {
                    if (slot.PlayerID == 0)
                    {
                        slot.SetButtonState(shopInfo, this);
                    }
                }
            }
        }
    }

    public void SelectPlayer(int playerID)
    {
        CurrentPlayer = playerID;
        foreach (var slot in _slotUIs)
        {
            slot.SetToggleCheck(slot.PlayerID == CurrentPlayer);
        }
    }
}

[System.Serializable]
public class ShopInfoPlayer
{
    public int PlayerID;
    public Sprite AvatarPlayer;
    public int Price;
    public bool Bought;
}