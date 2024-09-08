using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	[SerializeField] private float speed = 12f;

	private CharacterController characterController;
	private void Start()
	{
		characterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");

		Vector3 move = transform.right * x + transform.forward * z;

		characterController.Move(move * (speed * Time.deltaTime) + Physics.gravity * Time.deltaTime);
	}
}