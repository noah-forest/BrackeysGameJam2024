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
		float x = Input.GetAxisRaw("Horizontal");
		float z = Input.GetAxisRaw("Vertical");

		move = transform.right * x + transform.forward * z;
		characterController.Move(move.normalized * (speed * Time.deltaTime) + Physics.gravity * Time.deltaTime);
	}
}