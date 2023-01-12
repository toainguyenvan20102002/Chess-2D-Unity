
using UnityEngine;

public class CameraScale : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private void Start()
    {
        if (Screen.height == 1280 || Screen.height == 1920) _camera.fieldOfView = 112;
        else if (Screen.height == 2160) _camera.fieldOfView = 120;
    }
}
