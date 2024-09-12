using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    private float xRotation = 0f;

    private float mouseX;
    private float mouseY;

    private GameManager _gameManager;
    
    private void Start()
    {
        _gameManager = GameManager.singleton;
        
        Cursor.lockState = CursorLockMode.Locked;
        
        _gameManager.pauseGame.AddListener(UnLockCursor);
        _gameManager.resumeGame.AddListener(LockCursor);
    }

    private void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 80f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private static void UnLockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
}
