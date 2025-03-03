using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollown : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _virtualCamera;

    private PlayerController _player; 

    private void Start() 
    {
        Invoke(nameof(GetCameraFollown), 0.1f);   //Gọi phương thức GetCameraFollown sau 0.1 giây.
    }
    void GetCameraFollown()
    {
        _player = GameObject.FindObjectOfType<PlayerController>(); //Tìm trong toàn bộ scene một GameObject có gắn Component PlayerController.

        if (_player != null)     // Kiểm tra _player có tồn tại hay không
        {
            Transform pointCam = _player.transform.Find("PointCam");  // tìm một Transform con có tên "PointCam" bên trong _player.

            if (pointCam != null)
            {
                _virtualCamera.Follow = pointCam;
            }   
        }
    }
}
