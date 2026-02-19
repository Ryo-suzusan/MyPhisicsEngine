using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    // カメラ移動量
    [SerializeField, Range(0.1f, 10.0f)]
    float positionStep = 2.0f;

    // マウス感度
    [SerializeField, Range(30.0f, 150.0f)]
    float mouseSensitive = 90.0f;

    // カメラのtransform
    Transform camTransform;
    // マウスの始点
    Vector2 startMousePos;
    // カメラ回転の視点情報
    Quaternion presentCamRot;
    // 初期状態
    Quaternion initialCamRot;
    Vector3 initialCamPos;

    // キーボード状態
    Keyboard keyboard;
    // マウス状態
    Mouse mouse;

    private void Start()
    {
        this.gameObject.transform.rotation = Quaternion.identity;
        camTransform = this.gameObject.transform;

        initialCamRot = this.gameObject.transform.rotation;
        initialCamPos = this.gameObject.transform.position;
    }

    private void Update()
    {
        keyboard = Keyboard.current;
        mouse = Mouse.current;

        ResetCamera();
        CameraRotationControl();
        CameraPositionControl();
    }

    // カメラ初期化
    private void ResetCamera()
    {
        if (keyboard == null) return;

        if (keyboard.pKey.wasPressedThisFrame) 
        {
            startMousePos = mouse.position.ReadValue();
            this.gameObject.transform.rotation = initialCamRot;
            this.gameObject.transform.position = initialCamPos;
            Debug.Log("Cam Rotate: " + initialCamRot.ToString());
            Debug.Log("Cam Position: " + initialCamPos.ToString());
        }
    }

    // カメラ回転
    private void CameraRotationControl()
    {
        if (mouse == null) return;
        if (!mouse.rightButton.isPressed) return;

        if (mouse.rightButton.wasPressedThisFrame)
        {
            startMousePos = mouse.position.ReadValue();
            presentCamRot.x = camTransform.rotation.eulerAngles.x;
            presentCamRot.y = camTransform.rotation.eulerAngles.y;
        }

        float x = (startMousePos.x - mouse.position.ReadValue().x) / Screen.width;
        float y = (startMousePos.y - mouse.position.ReadValue().y) / Screen.height;

        float eulerX = presentCamRot.x + y * mouseSensitive;
        eulerX = Math.Min(90, eulerX);
        eulerX = Math.Max(-90, eulerX);
        float eulerY = presentCamRot.y + x * mouseSensitive;

        camTransform.rotation = Quaternion.Euler(eulerX, eulerY, 0);
    }

    // カメラ移動
    private void CameraPositionControl()
    {
        if (keyboard == null) return;
        if (!mouse.rightButton.isPressed) return;

        if (mouse.rightButton.wasPressedThisFrame)
        {
            startMousePos = mouse.position.ReadValue();
        }

        Vector3 camPos = camTransform.position;

        if (keyboard.dKey.isPressed)
        {
            camPos += camTransform.right * Time.deltaTime * positionStep;
        }
        if (keyboard.aKey.isPressed)
        {
            camPos -= camTransform.right * Time.deltaTime * positionStep;
        }
        if (keyboard.eKey.isPressed)
        {
            camPos += camTransform.up * Time.deltaTime * positionStep;
        }
        if (keyboard.qKey.isPressed)
        {
            camPos -= camTransform.up * Time.deltaTime * positionStep;
        }
        if (keyboard.wKey.isPressed)
        {
            camPos += camTransform.forward * Time.deltaTime * positionStep;
        }
        if (keyboard.sKey.isPressed)
        {
            camPos -= camTransform.forward * Time.deltaTime * positionStep;
        }

        camTransform.position = camPos;
    }
}
