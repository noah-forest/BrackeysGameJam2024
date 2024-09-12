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

    private bool gamePaused;

    private GameManager _gameManager;
    
    private void Start()
    {
        _gameManager = GameManager.singleton;

        LockCursor();
        
        _gameManager.pauseGame.AddListener(UnLockCursor);
        _gameManager.resumeGame.AddListener(LockCursor);
    }

    private void Update()
    {
        if (gamePaused) return;
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 80f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gamePaused = false;
    }

    private void UnLockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        gamePaused = true;
    }
}
