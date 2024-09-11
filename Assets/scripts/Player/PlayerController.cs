using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	private static readonly int speed = Animator.StringToHash("speed");
	[SerializeField] private float moveSpeed = 8f;

	Vector3 move;
	private Animator animator;

	private CharacterController characterController;
	private void Start()
	{
		characterController = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		float x = Input.GetAxisRaw("Horizontal");
		float z = Input.GetAxisRaw("Vertical");
		
		move = transform.right * x + transform.forward * z;
		characterController.Move(move.normalized * (moveSpeed * Time.deltaTime) + Physics.gravity * Time.deltaTime);

		//set up anims
		if (move == Vector3.zero)
		{
			//idle
			animator.SetFloat(speed, 0);
		}
		else
		{
			//walking
			animator.SetFloat(speed, 0.5f);
		}
	}
}