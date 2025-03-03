using _Game.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 80f;
    private void Update()
    {
        transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag =="Player")
        {
            GameManager.Instance.Coin++;
            GameUI.Instance.CoinDasbBoard++;
            SoundManager.Instance.PlaySFX(2);
            GameManager.Instance.SaveCoin();
            GameUI.Instance.UpdateCoin(GameManager.Instance.Coin);
            Destroy(gameObject);
        }
        if(other.gameObject.tag=="RaoChan")
        {
            Destroy(gameObject);
        }
    }
}
