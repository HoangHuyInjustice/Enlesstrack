using System;
using System.Collections;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public static GroundSpawner Instance;

    private Vector3 _nextSpawnPoint;               //Lưu vị trí của tile nền tiếp theo để spawn
    public GameObject[] GroundTilePrefabs;        //Danh sách map sẽ spawner ra theo thứ tự
    private int _currentIndex = 0;               // Chỉ số prefab hiện tại 
    public float ChangeInterval = 30f;          // Thời gian thay đổi map

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        for (int i = 0; i < 10; i++)                    //    Spawn 10 mảnh nền ban đầu để tạo sàn cho người chơi.
        {
            SpawnTile();
        }
        StartCoroutine(ChangeGroundTileRoutine());      //    Tự động thay đổi prefab sau một khoảng thời gian.
    }

    private IEnumerator ChangeGroundTileRoutine()       // Thay đổi Map theo thứ tự 
    {
        while (true)
        {
            yield return new WaitForSeconds(ChangeInterval); //giây(30s), prefab nền sẽ được thay đổi.
            _currentIndex = (_currentIndex + 1) % GroundTilePrefabs.Length; //Chỉ số _currentIndex tăng lên 1, giúp chuyển sang prefab tiếp theo trong mảng ,% GroundTilePrefabs.Length giúp quay lại prefab đầu tiên sau khi dùng hết danh sách.
        }
    }

    public void SpawnTile()                      // spawn map
    {
        if (GroundTilePrefabs.Length == 0) return;

        GameObject temp = Instantiate(GroundTilePrefabs[_currentIndex], _nextSpawnPoint, Quaternion.identity);
        _nextSpawnPoint = temp.transform.GetChild(1).position;
    }
}
