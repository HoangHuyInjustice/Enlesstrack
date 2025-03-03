using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionTest : MonoBehaviour
{
    public List<InfoPlayer> infoPlayers;
}

[System.Serializable]
public class InfoPlayer
{
    public int PlayerID;
    public Sprite AvatarPlayer;
    public int Price;
    public bool Bought;
}
