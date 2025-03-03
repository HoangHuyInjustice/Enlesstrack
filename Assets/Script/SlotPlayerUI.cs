using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotPlayerUI : MonoBehaviour// slot để hiển thị UI danh sách nhân vật trong shop
{
    [SerializeField]
    private Image _avatarPlayer;// hình ảnh nhân vật

    [SerializeField]
    private Image _toggleCheck;// kiểm tra xem đã mua hay chưa

    [SerializeField]
    private TextMeshProUGUI _priceTxt;// giá 

    [SerializeField]
    private Button _selectBtn;// nút chọn nhân vật 

    [SerializeField]
    private Button _buyBtn;// nút mua 

    public int PlayerID { get; private set; }

    public void SetUpUI(ShopInfoPlayer shopInfo)
    {
        _avatarPlayer.sprite = shopInfo.AvatarPlayer;
        _priceTxt.text = shopInfo.Price.ToString();
        PlayerID = shopInfo.PlayerID;
    }

    public void SetButtonState(ShopInfoPlayer shopInfo, SelectionPlayer selectionPlayer)
    {
        _selectBtn.gameObject.SetActive(shopInfo.Bought);
        _buyBtn.gameObject.SetActive(!shopInfo.Bought);
        _priceTxt.gameObject.SetActive(!shopInfo.Bought);

        _buyBtn.onClick.AddListener(() => BuyPlayer(shopInfo, selectionPlayer));
        _selectBtn.onClick.AddListener(() => selectionPlayer.SelectPlayer(PlayerID));
    }

    private void BuyPlayer(ShopInfoPlayer shopInfo, SelectionPlayer selectionPlayer)
    {
        if (GameManager.Instance.Coin >= shopInfo.Price)// nếu đủ tiền thì mua
        {
            GameManager.Instance.Coin -= shopInfo.Price;// trừ tiền 
            GameManager.Instance.SaveCoin();// lưu dữ liệu coin
            shopInfo.Bought = true;// đã mua rồi 
            selectionPlayer.SaveData(); // lưu trử dữ liệu trên máy người chơi là đã mua
            SetButtonState(shopInfo, selectionPlayer);
        }
        else
        {
            Debug.Log("Not enough gold!");// không đủ tiền để mua
        }
    }

    public void SetToggleCheck(bool isActive)// kiểm tra xem là đã chọn nhân vật đó hay chưa 
    {
        _toggleCheck.gameObject.SetActive(isActive);
    }
}
