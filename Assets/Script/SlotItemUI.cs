using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotItemUI : MonoBehaviour
{
    [SerializeField]
    private Image _avatarPlayer;// hình ảnh nhân vật

    [SerializeField]
    private TextMeshProUGUI _priceTxt;// giá 

    [SerializeField]
    private TextMeshProUGUI _descriptionTxt;// giá 

    [SerializeField]
    private Button _buyBtn;// nút mua 

    public int ItemID { get; private set; }

    public void SetUpUI(ShopItemInfo shopInfo)
    {
        _avatarPlayer.sprite = shopInfo.AvatarItem;
        _priceTxt.text = shopInfo.Price.ToString();
        _descriptionTxt.text = shopInfo.Description;
        ItemID = shopInfo.ItemID;
    }

    public void SetButtonState(ShopItemInfo shopInfo, ShopItemUI shopItemUI)
    {
        _buyBtn.onClick.RemoveAllListeners();
        _buyBtn.onClick.AddListener(() => BuyPlayer(shopInfo, shopItemUI));

        Text btnText = _buyBtn.GetComponentInChildren<Text>();

        if (shopInfo.ItemID == 0) // HP
        {
            if (!GameManager.Instance.CanBuyHP())
            {
                _buyBtn.interactable = false;
                btnText.color = Color.red;
                btnText.text = "Tối đa";
            }
            else
            {
                _buyBtn.interactable = true;
                btnText.color = Color.white;
                btnText.text = "Mua";
            }
        }
        else if (shopInfo.ItemID == 1) // Timer
        {
            if (!GameManager.Instance.CanBuyTimer())
            {
                _buyBtn.interactable = false;
                btnText.color = Color.red;
                btnText.text = "Tối đa";
            }
            else
            {
                _buyBtn.interactable = true;
                btnText.color = Color.white;
                btnText.text = "Mua";
            }
        }
    }


    private void BuyPlayer(ShopItemInfo shopInfo, ShopItemUI shopItemUI)
    {
        Debug.Log($"Mua Item: {shopInfo.ItemName}, Giá trị: {shopInfo.Amount}");

        if (GameManager.Instance.Coin >= shopInfo.Price)
        {
            GameManager.Instance.Coin -= shopInfo.Price;
            GameManager.Instance.SaveCoin();

            if (ItemID == 0) // HP
            {
                GameManager.Instance.AddShopHP(shopInfo.Amount);
                MainUI.Instance.UpdateCoin();
            }
            else if (ItemID == 1) // Timer
            {
                GameManager.Instance.AddShopTimer(shopInfo.Amount);
                GameManager.Instance.SaveTimer();
                MainUI.Instance.UpdateCoin();
            }

            SetButtonState(shopInfo, shopItemUI);

            Debug.Log("Đã mua thành công: " + shopInfo.ItemName);
        }
        else
        {
            Debug.Log("Không đủ tiền mua!");
        }
    }
}
