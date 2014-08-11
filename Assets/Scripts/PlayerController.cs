using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {

	public float gravity = 20;
	public float walkingSpeed = 8; 
	public float runningSpeed = 20; 

	public float acceleration = 30; 
	public float jumpHeight = 12;

	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove; 

	private Animator playerAnimator;

	private PlayerPhysics playerPhysics;


	// Use this for initialization
	void Start () {

		playerPhysics = GetComponent<PlayerPhysics>();
		playerAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {



		//check if the player collided before checking for the input 	
		if (playerPhysics.collided)
		{
			targetSpeed = 0;
			currentSpeed = 0;
		}
		if (playerPhysics.grounded) {
			amountToMove.y = 0;
			if (Input.GetButtonDown("Jump"))
				amountToMove.y = jumpHeight;

		}

		playerAnimator.SetFloat("Speed", Mathf.Abs(currentSpeed));
		//we set the animator float before the input 
		//input values 
		float speed = (Input.GetButton("Run"))? runningSpeed : walkingSpeed;
		targetSpeed = Input.GetAxisRaw ("Horizontal") * speed;
		currentSpeed = IncrementTowards (currentSpeed, targetSpeed, acceleration);

		amountToMove.x = currentSpeed;
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move (amountToMove * Time.deltaTime);
	}
	/**
	 * This function handles the speed changes
	 * <params>
	 * n: current speed
	 * target: target speed
	 * a: by how much you want to accelerate
	 * <params>
	 * 
	 * */
	private float IncrementTowards(float n, float target, float a)
	{
		if (n == target)
			return n;
		else{
			float dir = Mathf.Sign(target - n); //to get the sign of the acceleration
			n += a * Time.deltaTime * dir;

			// if n has passed the target speed then reset it to the target speed otherwise return n 
			return (dir == Mathf.Sign(target-n))? n:target; 
		}
	}
}
