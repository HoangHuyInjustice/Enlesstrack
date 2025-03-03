using _Game.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _playerPrefabs;

    private void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        SoundManager.Instance.StopSFX(1);
        SoundManager.Instance.PlaySFX(0);

        int selectedIndex = SelectionPlayer.Instance.CurrentPlayer;

        if (selectedIndex >= 0 && selectedIndex < _playerPrefabs.Length)
        {
            Instantiate(_playerPrefabs[selectedIndex], transform.position, Quaternion.identity);
        }
    }
}
