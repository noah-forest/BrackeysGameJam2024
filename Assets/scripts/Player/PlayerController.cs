using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	[SerializeField] private float speed = 12f;

	Vector3 move;

	private CharacterController characterController;
	private void Start()
	{
		characterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");

		move = transform.right * x + transform.forward * z;
		characterController.Move(move * (speed * Time.deltaTime) + Physics.gravity * Time.deltaTime);
	}
}