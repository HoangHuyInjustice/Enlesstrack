using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaoChan : MonoBehaviour
{
    PlayerController _player;

    private void Awake()
    {
        _player =GameObject.FindObjectOfType<PlayerController>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
           // _player.Die();
        }
    }
}
