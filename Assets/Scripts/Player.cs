using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private float runSpeed = 25f;
    [SerializeField] private float jumpSpeed = 25f;
    [SerializeField] private float climbSpeed = 5f;

    private float startingGravity;

    private Rigidbody2D myRigidBody;
    private BoxCollider2D feet;
    private Animator myAnimator;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        feet = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();

        startingGravity = myRigidBody.gravityScale;
    }

    void Update()
    {
        Run();
        Jump();
        ClimbLadder();

        FlipSprite();
    }

    private void Run()
    {
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        myRigidBody.velocity = new Vector2(horizontal * runSpeed, myRigidBody.velocity.y);

        bool hasHorizontalMovement = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
            myAnimator.SetBool("Running", hasHorizontalMovement);
    }

    private void Jump()
    {
        if(!feet.IsTouchingLayers(LayerMask.GetMask("Ground")))
            return;

        if(CrossPlatformInputManager.GetButtonDown("Jump"))
            myRigidBody.velocity += new Vector2(0f, jumpSpeed);
    }

    private void ClimbLadder()
    {
        if(!feet.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody.gravityScale = startingGravity;
            return;
        }

        float vertical = CrossPlatformInputManager.GetAxis("Vertical");
        myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, vertical * climbSpeed);
        myRigidBody.gravityScale = 0f;

        bool hasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", hasVerticalSpeed);
    }

    private void FlipSprite()
    {
        bool hasHorizontalMovement = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;

        if(hasHorizontalMovement)
            transform.localScale = new Vector3(Mathf.Sign(myRigidBody.velocity.x), transform.localScale.y, transform.localScale.z);
    }
}
