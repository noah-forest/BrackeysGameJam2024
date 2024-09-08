using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController cc;
    [SerializeField] private GameObject player;
    [SerializeField] private Camera cam;
    [SerializeField] private float Sensitivity;

    [SerializeField] private float speed, walk, run, crouch;

    private Vector3 crouchScale, normalScale;

    public bool isMoving, isCrouching, isRunning;

    public Vector3 cameraOffset = new Vector3(0, 1.5f, 0);

    private float X, Y;

    private void Start()
    {
        speed = walk;
        crouchScale = new Vector3(1, .75f, 1);
        normalScale = new Vector3(1, 1, 1);
        cc = GetComponent<CharacterController>();
        cc.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        #region Camera Limitation Calculator
        //Camera limitation variables
        const float MIN_Y = -60.0f;
        const float MAX_Y = 70.0f;

        X += Input.GetAxis("Mouse X") * Sensitivity;
        Y -= Input.GetAxis("Mouse Y") * Sensitivity;

        if (Y < MIN_Y)
            Y = MIN_Y;
        else if (Y > MAX_Y)
            Y = MAX_Y;
        #endregion
        cam.transform.localRotation = Quaternion.Euler(Y, X, 0.0f);
        cam.transform.position = transform.position + cameraOffset;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 forward = cam.transform.forward * vertical;
        Vector3 right = cam.transform.right * horizontal;


        cc.SimpleMove((forward + right) * speed);
        // Determines if the speed = run or walk
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = run;
            isRunning = true;
        }
        //Crouch
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
            isRunning = false;
            speed = crouch;
            player.transform.localScale = crouchScale;
        }
        else
        {
            isRunning = false;
            isCrouching = false;
            speed = walk;
            player.transform.localScale = normalScale;
        }
        // Detects if the player is moving.
        // Useful if you want footstep sounds and or other features in your game.
        isMoving = cc.velocity.sqrMagnitude > 0.0f ? true : false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.position + cameraOffset, new Vector3(0.1f, 0.1f, 0.1f));
    }
}