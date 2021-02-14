using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleCameraFollow : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public Transform playerTransform;

    private void Start()
    {
        cameraFollow.SetupCamera(() => playerTransform.position);
        cameraFollow.SetCameraOfsetFunc(() => new Vector3(0f, 15f, -11f));
    }
}
