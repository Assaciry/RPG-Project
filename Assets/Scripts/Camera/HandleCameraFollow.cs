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
    }
}
