using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	[SerializeField] float FLAP_FORCE;
	[SerializeField] int maxSpeed = 20;
	[SerializeField] float SIDE_SPEED = 6f;
	[SerializeField] float GLIDE_SPEED = 0.3f;
	public float glideConstant = 2f;
	public float NORMAL_DRAG;

	bool grounded;

	public enum WingState
	{
		Flapping,
		Resting,
		Gliding
	}

	public WingState wingState = WingState.Resting;

	bool wantGlide;

	private Animator animator;
	
	// Use this for initialization
	void Start()
	{
		animator = this.GetComponent<Animator>();
	}

	bool facingRight = false;

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

	void Update()
	{
		if(Input.GetButton("Flap"))
		{
			if(wingState == WingState.Resting)
			{
				rigidbody2D.AddForce(Vector2.up * FLAP_FORCE);
				wingState = WingState.Flapping;
				animator.SetTrigger("Flapping");
				SetGroundState(false);
			}

			//If the user is holding flap button while flap animation plays, we should glide
			else if(wingState == WingState.Flapping && !grounded)
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
		if(grounded)
			return;
		if(!wantGlide)
			rigidbody2D.velocity += new Vector2(Mathf.Min (horizontalInput * SIDE_SPEED, maxSpeed), 0);
		else rigidbody2D.velocity += new Vector2(Mathf.Min (horizontalInput * GLIDE_SPEED, maxSpeed), Mathf.Pow((Mathf.Abs (rigidbody2D.velocity.x) / glideConstant / maxSpeed), 2));
	}

	void SetGroundState(bool grounded)
	{
		this.grounded = grounded;
		animator.SetBool("Grounded", grounded);

		if(grounded){
			animator.SetBool("Gliding", false);
			wingState = WingState.Resting;
		}
			
	}

	const int GroundLayer = 11;
	void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.layer == GroundLayer)
			SetGroundState(true);
	}

	void OnCollisioneExit2D(Collision2D collision)
	{
		if(collision.gameObject.layer == GroundLayer)
			SetGroundState(false);
	}

	//Called by animation.
	public void FlapAnimationFinished()
	{
		if(wantGlide && !grounded)
		{
			wingState = WingState.Gliding;
			rigidbody2D.drag = NORMAL_DRAG;
		}
		else
		{
			wingState = WingState.Resting;
		}
	}

	public bool isFacingRight() {
		return facingRight;
	}
}

/*
 * 
idealDrag = maxAcceleration / terminalVelocity;
rigidbody.drag = idealDrag / ( idealDrag * Time.fixedDeltaTime + 1 );
 */