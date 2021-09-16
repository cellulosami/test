using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    public PlayerMovement movement;
    public PlayerBubbleMovement bubbleMovement;
    private Rigidbody2D rigibody;

    public float moveSpeed = 40f;
    float horizontalInput = 0f;
    float verticalInput = 0f;
    bool jump = false;
    bool jumping = false;
    public float normalGravity = 6f;
    public PhysicsMaterial2D bouncyMaterial;

    public float bubbleSpeed = 450f;
    bool toggleBubble = false;
    public bool bubbleActive = false;
    public GameObject playerBubble;
    void Start()
    {
        rigibody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }

        jumping = Input.GetKey(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            toggleBubble = true;
        }
    }

    private void FixedUpdate()
    {
        if (toggleBubble)
        {
            ToggleBubble();
        }

        //Choose movement method
        if (bubbleActive)
        {
            bubbleMovement.Move(horizontalInput * Time.fixedDeltaTime * bubbleSpeed, verticalInput * Time.fixedDeltaTime * bubbleSpeed);
        }
        else
        {
            movement.Move(horizontalInput * Time.fixedDeltaTime * moveSpeed, jump, jumping);
        }

        jump = false;
        toggleBubble = false;
    }

    private void ToggleBubble()
    {
        bubbleActive = !bubbleActive;
        playerBubble.SetActive(bubbleActive);
        if (bubbleActive)
        {
            rigibody.gravityScale = 0;
            rigibody.sharedMaterial = bouncyMaterial;
        }
        else
        {
            rigibody.gravityScale = normalGravity;
        }
    }
}
