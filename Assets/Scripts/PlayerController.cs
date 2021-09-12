using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	//General Variables
	[SerializeField] private float m_JumpForce = 28f;                          // Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings

	public float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	public bool canJump;            // Whether or not the player can jump.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	public bool isGrounded;            // Whether or not the player is grounded.
	private Vector2 targetVelocity = Vector2.zero;

	//Slope Variables
	public float checkSlopeDistance;
	private float slopeDownAngle;
	private Vector2 slopeNormalPerpendicular;
	public bool isOnSlope;
	public PhysicsMaterial2D zeroFriction;
	public PhysicsMaterial2D fullFriction;

	//Jumping
	public float jumpBufferTime = 0.3f;
	private float jumpBufferTimeRemaining;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		CheckGround();
		CheckSlope();
	}

	private void CheckGround()
    {
		isGrounded = (Physics2D.OverlapCircle(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround));
		if (isGrounded)
        {
			jumpBufferTimeRemaining = jumpBufferTime;
        }

		jumpBufferTimeRemaining -= Time.fixedDeltaTime;
		canJump = jumpBufferTimeRemaining > 0;
	}

	private void CheckSlope()
    {
		CheckSlopeVertical();
    }

	private void CheckSlopeVertical()
    {
		RaycastHit2D hit = Physics2D.Raycast(m_GroundCheck.position, Vector2.down, checkSlopeDistance, m_WhatIsGround);

		if (hit)
        {
			slopeNormalPerpendicular = Vector2.Perpendicular(hit.normal).normalized;
			slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

			if(slopeDownAngle == 0)
            {
				isOnSlope = false;
            }
			else
            {
				isOnSlope = true;
            }

			Debug.DrawRay(hit.point, slopeNormalPerpendicular, Color.magenta);
			Debug.DrawRay(hit.point, hit.normal, Color.red);
        }
    }

	public void Move(float move, bool jump, bool jumping)
	{
		//Don't slide down slopes
		if (move == 0 && isGrounded)
		{
			m_Rigidbody2D.sharedMaterial = fullFriction;
		}
		else
		{
			m_Rigidbody2D.sharedMaterial = zeroFriction;
		}

		//Running
		if (isGrounded && !isOnSlope)
        {
			targetVelocity.Set(move, 0f);
		}
		else if(isGrounded && isOnSlope)
        {
			targetVelocity.Set(move * -slopeNormalPerpendicular.x, move * -slopeNormalPerpendicular.y);
		}
		else
        {
			targetVelocity.Set(move, m_Rigidbody2D.velocity.y);
		}
		
		m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
		m_Rigidbody2D.velocity = targetVelocity;

		//Jumping
		if (canJump && jump)
		{
			canJump = false;
			jumpBufferTimeRemaining = 0f;
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
		}

		//Cancel Jump
		if (m_Rigidbody2D.velocity.y > 0f && !jumping)
        {
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f);
        }

		//Correct Facing
		if (move > 0 && !m_FacingRight)
		{
			Flip();
		}
		else if (move < 0 && m_FacingRight)
		{
			Flip();
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
