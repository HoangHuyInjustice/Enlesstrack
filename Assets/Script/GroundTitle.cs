using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundTitle : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _raochanPrefabs;// Rào chắn

    [SerializeField]
    private GameObject _coinPrefabs;// Coin 

    [SerializeField]
    private GameObject[] _itemSO; //Item: Coin....

    private void Start()
    {
        SpawnRaoChan();
        SpawnCoin();
        SpawnItemSO();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")   // khi rời khỏi ground đó
        {
            GroundSpawner.Instance.SpawnTile(); // sẽ spawn ra ground tiếp theo
            Destroy(gameObject, 5);             // sau 5s thì nó sẽ xóa các GROUNd đó đi
        }
    }

    void SpawnItemSO()                         // Spawn ra Item
    {
        int randomChance = Random.Range(0, 100);// tỉ lệ
        if (randomChance < 50)
        {
            int itemIndex = Random.Range(0, _itemSO.Length);
            GameObject tempItem = Instantiate(_itemSO[itemIndex], transform);
            tempItem.transform.position = RanDomPos(GetComponent<Collider>());
        }
    }

    void SpawnRaoChan()                      // Spawn Rào chắn
    {
        int raoChanChiMuc = Random.Range(2, 5);
        Transform spawnPoint = transform.GetChild(raoChanChiMuc).transform; // lấy transfrom là con của game object GroundTitle spawn theo vị trí của point
        int prefabIndex = Random.Range(0, _raochanPrefabs.Length);
        Instantiate(_raochanPrefabs[prefabIndex], spawnPoint.position, Quaternion.identity, transform);
    }

    void SpawnCoin()                        // Spawn Coin
    {
        GameObject temp = Instantiate(_coinPrefabs, transform); //
        temp.transform.position = RanDomPos(GetComponent<Collider>());
    }

    Vector3 RanDomPos(Collider collider)   // Khoảng cách để spawn Coin
    {
        Vector3 point = new Vector3(
            Random.Range(collider.bounds.min.x, collider.bounds.max.x),
            Random.Range(collider.bounds.min.y, collider.bounds.max.y),
            Random.Range(collider.bounds.min.z, collider.bounds.max.z));
        if (point != collider.ClosestPoint(point))
        {
            point = RanDomPos(collider);
        }
        point.y = 1;
        return point;
    }
}
