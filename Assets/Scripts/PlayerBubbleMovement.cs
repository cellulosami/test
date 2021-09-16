using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBubbleMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 targetVelocity = Vector2.zero;
    [SerializeField] private float movementSmoothing = 1f;
    private Vector3 m_Velocity = Vector3.zero;
    public float maxSpeed = 20f;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move(float horizontalMove, float verticalMove)
    {
        targetVelocity.Set(horizontalMove, verticalMove);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, movementSmoothing);
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}
