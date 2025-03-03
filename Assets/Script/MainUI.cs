using _Game.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField]
    private Image _iconOn;

    [SerializeField]
    private Image _iconOff;

    [SerializeField]
    private Button _turnSoundOnandOff;

    [SerializeField]
    private TextMeshProUGUI _nameUserTxt;

    [SerializeField]
    private TMP_InputField _nameInputField;

    [SerializeField]
    private TextMeshProUGUI _amountCoinTxt;

    [SerializeField]
    private Button _startGameBtn;

    [SerializeField]
    private Button _selectedNameBtn;

    [SerializeField]
    private GameObject _panelNamePopup;

    private string UserNameKey = "UserName";

    public static MainUI Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SoundManager.Instance.PlaySFX(1);

        _turnSoundOnandOff.onClick.AddListener(ToggleSound);
        UpdateSoundState();

        _startGameBtn.onClick.AddListener(StartGame);
        _selectedNameBtn.onClick.AddListener(SetUserName);

        CheckUserName();
        UpdateCoin();
    }

    public void ToggleSound()
    {
        SoundManager.Instance.IsSound = !SoundManager.Instance.IsSound;
        UpdateSoundState();
    }

    public void UpdateSoundState()
    {
        _iconOn.gameObject.SetActive(SoundManager.Instance.IsSound);
        _iconOff.gameObject.SetActive(!SoundManager.Instance.IsSound);

        for (int i = 0; i < SoundManager.Instance.soundEffects.Length; i++)
        {
            SoundManager.Instance.soundEffects[i].volume = SoundManager.Instance.IsSound ? SoundManager.Instance.OriginalVolumes[i] : 0f;
        }
    }

    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void UpdateCoin()
    {
        _amountCoinTxt.text = GameManager.Instance.Coin.ToString("N0");
    }

    private void CheckUserName()
    {
        if (PlayerPrefs.HasKey(UserNameKey))
        {
            string userName = PlayerPrefs.GetString(UserNameKey); // Save tên người chơi
            _nameUserTxt.text = userName;
            _panelNamePopup.SetActive(false);
        }
        else
        {
            _panelNamePopup.SetActive(true);
        }
    }

    private void SetUserName() // Save tên người chơi
    {
        string inputName = _nameInputField.text;

        if (!string.IsNullOrEmpty(inputName))
        {
            PlayerPrefs.SetString(UserNameKey, inputName); // Save tên người chơi
            PlayerPrefs.Save(); // Save tên người chơi

            _nameUserTxt.text = inputName;
            _panelNamePopup.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Name input is empty. Please enter a valid name.");
        }
    }
}
