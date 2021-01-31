using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Func<Vector3> GetCameraFollowPosFunc;
    private Func<Vector3> GetCameraOfsetFunc;

    public void SetupCamera(Func<Vector3> GetCameraFollowPosFunc)
    {
        this.GetCameraFollowPosFunc = GetCameraFollowPosFunc;  
    }

    public void SetCameraOfsetFunc(Func<Vector3> GetCameraOfsetFunc)
    {
        this.GetCameraOfsetFunc = GetCameraOfsetFunc;
    }
    
    void Update()
    {
        CameraFollowPlayer();
    }

    private void CameraFollowPlayer()
    {
        Vector3 cameraFollowPos = GetCameraFollowPosFunc() + GetCameraOfsetFunc();


        // Smooth camera movement
        float followSpeed = 1.5f;
        Vector3 direction = (cameraFollowPos - transform.position).normalized;
        float distance = Vector3.Distance(cameraFollowPos, transform.position);

        // Cancel camera jitter
        if ( distance > 0f )
        {
             Vector3 newCamPos = transform.position + direction * followSpeed * distance * Time.deltaTime;

            bool isOvershotTarget = Vector3.Distance(newCamPos, cameraFollowPos) > distance;

            if(isOvershotTarget)
            {
                newCamPos = cameraFollowPos;
            }

            transform.position = newCamPos;
        }
    }

    public void RotateAroundPlayer(float direction)
    {
        
    }
}
