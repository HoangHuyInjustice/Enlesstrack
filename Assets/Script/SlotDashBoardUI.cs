using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotDashBoardUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _topTxt;

    [SerializeField]
    private Text _coinaAchievedTxt;

    [SerializeField]
    private Text _timerAchievedTxt;


    public void SetUpUI(int coinAchieved,float timerAchieved)
    {
        _timerAchievedTxt.text = "Thời gian: " + timerAchieved.ToString("N0") + "s";
        _coinaAchievedTxt.text = "Tiền: " + coinAchieved.ToString("N0");
    }

    public void SetTextColor(Color color)
    {
        _coinaAchievedTxt.color = color;
        _timerAchievedTxt.color = color;
    }

    public void SetTopText(string text)
    {
        _topTxt.text = text;
    }
}
