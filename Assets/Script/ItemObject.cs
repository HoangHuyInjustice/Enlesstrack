using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField]
    private ItemSO _itemSO;

    [SerializeField]
    private float _rotationSpeed = 80f;
    private void Update()
    {
        transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")// KHI VA chạm với gameobject có tag Player thì nó sẽ công chỉ số.
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                if (_itemSO.BonusHP > 0)
                {
                    player.IncreaseHP(_itemSO.BonusHP);// cộng hp
                }

                if (_itemSO.BonusSpeed > 0)
                {
                    player.ActivateSpeedBonus(_itemSO.BonusSpeed, 30f); // cộng tốc độ 30s 
                }

                if (_itemSO.BonusDef > 0)
                {
                    player.ActivateDefBonus(_itemSO.BonusDef, 15f);// cộng giáp xuyên tưởng 15s
                }
            }

            Destroy(gameObject);
        }
    }
}
