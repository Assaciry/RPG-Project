using UnityEngine;

public class KeyboardControls : MonoBehaviour
{
    public KeyCode cameraRotateLeftKey = KeyCode.Q;
    public KeyCode cameraRotateRightKey = KeyCode.E;

    public CameraFollow cameraFollow;

    private void Update()
    {
        KeyboardInput();
    }

    private void KeyboardInput()
    {
        if(Input.GetKey(cameraRotateRightKey))
        {
            cameraFollow.RotateAroundPlayer(-1);
        }

        if (Input.GetKey(cameraRotateLeftKey))
        {
            cameraFollow.RotateAroundPlayer(1);
        }
    }
}
