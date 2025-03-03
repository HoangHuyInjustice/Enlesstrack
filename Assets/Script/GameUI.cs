using _Game.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    [SerializeField]
    private SlotDashBoardUI _slotDashBoardUI;

    [SerializeField]
    private DataCoinAndTimer _dataCoinAndTimer;

    [SerializeField]
    private GameObject _statHPImg;

    [SerializeField]
    private Transform _container;

    [SerializeField]
    private TextMeshProUGUI _amountCoinTxt;

    [SerializeField]
    private TextMeshProUGUI _timerTxt;

    [SerializeField]
    private TextMeshProUGUI _timerSpeedTxt;

    [SerializeField]
    private TextMeshProUGUI _timerDefTxt;

    [SerializeField]
    private TextMeshProUGUI _goldTxt;

    [SerializeField]
    private Button _loadGameBtn;

    [SerializeField]
    private GameObject _panelLoseGame;

    [SerializeField]
    private GameObject _iconSpeed;

    [SerializeField]
    private GameObject _iconDef;

    [SerializeField]
    private GameObject _panelSetting;

    [SerializeField]
    private Transform _containerDashBoard;

    public float Timer = 120f;

    public int CoinDasbBoard;

    public static GameUI Instance;

    private bool _isSetting;

    [SerializeField]
    private PlayerController _playerController;

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
            return;
        }
        Timer += GameManager.Instance.Timer * 30f; // Cộng thêm 10 giây vào Timer của GameUI
       GameManager.Instance.Timer = 0;            // Reset Timer của GameManager
        GameManager.Instance.SaveTimer();
    }

    private void Start()
    {
        StartCoroutine(TimerCountdown());
        _loadGameBtn.onClick.AddListener(LoadGame);
    }

    private void GetPlayer()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Setting();
        }
        Invoke(nameof(GetPlayer), 3f);
    }

    public void UpdateCoin(int amount)
    {
        _amountCoinTxt.text = amount.ToString("N0");
    }

    public void UpdateHP(int currentHP, int maxHP)            
    {
        foreach (Transform child in _container)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < currentHP; i++) 
        {
            Instantiate(_statHPImg, _container);
        }
    }

    public IEnumerator TimerCountdown()
    {
        while (Timer > 0)
        {
            Timer -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(Timer / 60);
            int seconds = Mathf.FloorToInt(Timer % 60);
            _timerTxt.text = string.Format("{0}:{1:00}", minutes, seconds);
            yield return null;
        }
        _panelLoseGame.SetActive(true);
        Timer = 0;
        Time.timeScale = 0;
        SoundManager.Instance.StopSFX(0);
        SoundManager.Instance.PlaySFX(4);
        _timerTxt.text = "0:00";
        _goldTxt.text = "+" + CoinDasbBoard;
    }

    public void ShowSpeedTimer(float duration)
    {
        _iconSpeed.gameObject.SetActive(true);
        StartCoroutine(SpeedTimerCountdown(duration));
    }

    public void ShowDefTimer(float duration)
    {
        _iconDef.gameObject.SetActive(true);
        StartCoroutine(DefTimerCountdown(duration));
    }

    public void HideSpeedTimer()
    {
        _iconSpeed.gameObject.SetActive(false);
    }

    public void HideDefTimer()
    {
        _iconDef.gameObject.SetActive(false);
    }

    private IEnumerator SpeedTimerCountdown(float duration)
    {
        float timer = duration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            int seconds = Mathf.FloorToInt(timer);
            _timerSpeedTxt.text = string.Format("X2: {0}s", seconds);
            yield return null;
        }
        _timerSpeedTxt.text = ": 0s";
    }

    private IEnumerator DefTimerCountdown(float duration)
    {
        float timer = duration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            int seconds = Mathf.FloorToInt(timer);
            _timerDefTxt.text = string.Format(": {0}s", seconds);
            yield return null;
        }
        _timerDefTxt.text = ": 0s";
    }

    private void Setting()
    {
        _isSetting = !_isSetting;
        _panelSetting.SetActive(_isSetting);

        if (_isSetting)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void OnPlayerDeath()          // gọi hàm này để bắt đầu thêm timer và coin đã đạt được
    {
        if (_playerController != null && !_playerController.IsAlive)
        {
            if (_dataCoinAndTimer == null)
            {
                _dataCoinAndTimer = new DataCoinAndTimer();
            }

            // Lưu dữ liệu mới vào danh sách
            _dataCoinAndTimer.coin.Add(CoinDasbBoard);
            _dataCoinAndTimer.timer.Add(120f - Timer); // Thời gian đã sống

            // Sắp xếp danh sách theo coin giảm dần
            List<(int coin, float time)> sortedData = new List<(int, float)>();
            for (int i = 0; i < _dataCoinAndTimer.coin.Count; i++)
            {
                sortedData.Add((_dataCoinAndTimer.coin[i], _dataCoinAndTimer.timer[i]));
            }
            sortedData.Sort((a, b) => b.coin.CompareTo(a.coin)); // Sắp xếp theo coin giảm dần

            // Xóa UI cũ
            foreach (Transform child in _containerDashBoard)
            {
                Destroy(child.gameObject);
            }

            // Hiển thị dữ liệu mới
            for (int i = 0; i < sortedData.Count; i++)
            {
                SlotDashBoardUI newSlot = Instantiate(_slotDashBoardUI, _containerDashBoard);// Instantiate ra slot khi mà đã đạt đc timer + coin
                newSlot.SetUpUI(sortedData[i].coin, sortedData[i].time);
                newSlot.SetTopText($"Top {i + 1}");

                if (i == 0)
                    newSlot.SetTextColor(Color.yellow);
                else
                    newSlot.SetTextColor(Color.white);
            }
        }
    }

    private void LoadGame() // bắt đầu lại game
    {
        Time.timeScale = 1;
        _panelLoseGame.SetActive(false);
        SceneManager.LoadScene("GamePlay");
        Timer = 180f;
        StartCoroutine(TimerCountdown());
    }

    public void Continue()
    {
        Time.timeScale = 1;
        _isSetting = false;
        _panelSetting.SetActive(false);
    }

    public void Main()
    {
        SoundManager.Instance.StopSFX(Random.Range(0,1));
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
        Destroy(gameObject);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    [System.Serializable]
    public class DataCoinAndTimer// hiển thị data thời gian và coin đã đạt được
    {
        public List<int> coin;
        public List<float> timer;

        public DataCoinAndTimer()
        {
            coin = new List<int>();
            timer = new List<float>();
        }
    }
}
