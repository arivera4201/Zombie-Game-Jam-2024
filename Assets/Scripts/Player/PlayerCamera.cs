using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Camera playerCam;
    private float xRot;
    private GameObject arm;
    private Vector3 armOriginalPos;
    public float sensitivity = 10.0f;
    public bool enableCamera;

    private float interpolationFrames = 5.0f;

    public void Start()
    {
        enableCamera = false;
        arm = GameObject.FindGameObjectWithTag("Arm");
        armOriginalPos = arm.transform.localPosition;
        arm.SetActive(false);
    }

    public void EnableCameraStuff() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void DisableCameraStuff() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CameraMove(Vector2 input)
    {
        if (enableCamera) {
            xRot -= (input.y * Time.deltaTime) * sensitivity;
            xRot = Mathf.Clamp(xRot, -90f, 90f);
            playerCam.transform.localRotation = Quaternion.Euler(xRot, 0, 0);
            transform.Rotate(Vector3.up * (input.x * Time.deltaTime) * sensitivity);

            ArmMovement(input);
        }
    }

    void ArmMovement(Vector2 input)
    {
        Vector3 armTargetPosition = new Vector3(armOriginalPos.x + Mathf.Clamp((input.x / 500.0f), -1.0f, 1.0f), armOriginalPos.y + Mathf.Clamp((input.y / 500.0f), -1.0f, 1.0f), armOriginalPos.z);
        arm.transform.localPosition = Vector3.Slerp(arm.transform.localPosition, armTargetPosition, Time.deltaTime * interpolationFrames);

        Quaternion armTargetRotation = Quaternion.Euler(input.y / 2.0f, 0, -input.x / 5.0f);
        arm.transform.GetChild(0).transform.localRotation = Quaternion.Slerp(arm.transform.GetChild(0).transform.localRotation, armTargetRotation, Time.deltaTime * interpolationFrames);
    }
}
