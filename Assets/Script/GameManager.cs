using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour    // quản lí Game 
{
    public int Coin;

    public int HP;

    public int Timer;

    private int MaxHPBuys = 3;             // Số lần HP có thể mua tối đa
    private int MaxTimerBuys = 10000;      // Số lần Timer có thể mua tối đa
    private int currentHPBuys = 0;         // Số lần HP đã mua
    private int currentTimerBuys = 0;      // Số lần Timer đã mua

    public static GameManager Instance;

    private void Awake()          
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()  
    {
        LoadCoin();
        LoadShopHP();
        LoadTimer();
    }

    public void AddShopHP(int amount) // thêm số lượng Hp
    {
        if (currentHPBuys < MaxHPBuys)
        {
            HP += amount;
            SaveShopHP();
            currentHPBuys++;
        }
    }

    public void AddShopTimer(int amount) // thêm số lượng thời gian
    {
        if (currentTimerBuys < MaxTimerBuys)
        {
            Timer += amount;
            SaveTimer();
            currentTimerBuys++;
        }
    }

    public int GetCurrentHPBuys() => currentHPBuys;
    public int GetCurrentTimerBuys() => currentTimerBuys;

    public bool CanBuyHP() => currentHPBuys < MaxHPBuys;
    public bool CanBuyTimer() => currentTimerBuys < MaxTimerBuys;

    public void SaveShopHP()                  // Lưu trử coin người chơi
    {
        PlayerPrefs.SetInt("ShopHP", HP);
        PlayerPrefs.Save();
    }

    public void SaveTimer()                 // thời gian
    {
        PlayerPrefs.SetInt("PlayerTimer", Timer);
        PlayerPrefs.Save();
    }

    public void SaveCoin()                   // Lưu trử coin người chơi 
    {
        PlayerPrefs.SetInt("PlayerCoin", Coin);
        PlayerPrefs.Save();
    }

    public void LoadCoin()                  // Load Coin người chơi khi bắt đầu chạy game
    {
        if (PlayerPrefs.HasKey("PlayerCoin"))
        {
            Coin = PlayerPrefs.GetInt("PlayerCoin");
            if (GameUI.Instance !=null)
            {
                GameUI.Instance.UpdateCoin(Coin);
            }
        }
        else
        {
            Coin = 0;
        }
    }

    public void LoadTimer() // load số lượng thời gian dã mua
    {
        if (PlayerPrefs.HasKey("PlayerTimer"))
        {
            Timer = PlayerPrefs.GetInt("PlayerTimer");
        }
        else
        {
            Timer = 0;
        }
    }

    public void LoadShopHP()                // Load Coin người chơi khi bắt đầu chạy game
    {
        if (PlayerPrefs.HasKey("ShopHP"))
        {
            HP = PlayerPrefs.GetInt("ShopHP");
        }
        else
        {
            HP = 0;
        }
    }
}
