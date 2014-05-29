using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float FLAP_FORCE;

	public enum WingState
	{
		Flapping,
		Resting,
		Gliding
	}

	public float NORMAL_DRAG = .7f;

	public WingState wingState = WingState.Resting;

	bool wantGlide;

	private Animator animator;
	
	// Use this for initialization
	void Start()
	{
		animator = this.GetComponent<Animator>();
	}

	bool facingRight = true;

	void DetermineDirection(float direction)
	{
		if(direction < 0f && facingRight || direction > 0f && !facingRight)
		{
			facingRight = !facingRight;
			
			Vector3 localScale = transform.localScale;
			localScale.x *= -1f;
			transform.localScale = localScale;
		}
	}

	float horizontalInput;
	float SIDE_SPEED = 5f;
	void Update()
	{
		if(Input.GetButton("Flap"))
		{
			if(wingState == WingState.Resting)
			{
				rigidbody2D.AddForce(Vector2.up * FLAP_FORCE);
				wingState = WingState.Flapping;
				animator.SetTrigger("Flapping");
			}

			//If the user is holding flap button while flap animation plays, we should glide
			else if(wingState == WingState.Flapping)
			{
				wantGlide = true;
				animator.SetBool("Gliding", true);
			}
		}
		else if(wantGlide)
		{
			//Player no longer wants to be gliding, since button was released
			wantGlide = false;
			animator.SetBool("Gliding", false);
			if(wingState == WingState.Gliding){
				rigidbody2D.drag = NORMAL_DRAG;
				wingState = WingState.Resting;
			}
		}

		//Sideways movement
		horizontalInput = Input.GetAxisRaw("Horizontal");
		DetermineDirection(horizontalInput);

	}

	void FixedUpdate()
	{
		rigidbody2D.velocity = new Vector2(horizontalInput * SIDE_SPEED, rigidbody2D.velocity.y);
	}

	public void FlapAnimationFinished()
	{
		if(wantGlide)
		{
			wingState = WingState.Gliding;
			rigidbody2D.drag = 7f;
		}
		else
		{
			wingState = WingState.Resting;
		}
	}
}

/*
 * 
idealDrag = maxAcceleration / terminalVelocity;
rigidbody.drag = idealDrag / ( idealDrag * Time.fixedDeltaTime + 1 );
 */