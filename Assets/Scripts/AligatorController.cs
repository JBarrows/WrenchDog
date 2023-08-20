using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AligatorController : MonoBehaviour
{
    public float maxHorizontalSpeed = 6.0f;
    public float horizontalInputScalar = 5.0f;
    public float jumpForce = 5.0f;
    public float riseTime = 0.5f; // How long gravity is suspended as you hold the jump key

    int facingDirection = 1; // -1: left, 0: away/towards, 1: right
    bool isJumping = false;
    float jumpStartTime = 0.0f;
    new Rigidbody2D rigidbody;

    public bool Jumping
    {
        get { return isJumping; }
        set { 
            isJumping = value;
            if (!rigidbody)
                return;

            if (isJumping) {
                rigidbody.drag = 2.5f;
                rigidbody.gravityScale = 0.1f;
            } else {
                rigidbody.gravityScale = 1.0f;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetHorizontalInput();
        GetJumpInput();
    }

    void GetHorizontalInput()
    {
        var hInput = Input.GetAxis("Horizontal");
        if (hInput > 0) {
            // Pressing right
            if (rigidbody.velocity.x >= maxHorizontalSpeed) {
                return; // EXIT: Already going too fast in this direction
            } else if (facingDirection < 0) {
                facingDirection = 1;
                transform.eulerAngles = Vector3.zero;
            }
        } else if (hInput < 0) {
            // Pressing left
            if (rigidbody.velocity.x <= -maxHorizontalSpeed) {
                return; // EXIT: Already going too fast in this direction
            } else if (facingDirection > 0) {
                facingDirection = -1;
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }

        rigidbody.AddForce(Vector2.right * hInput * horizontalInputScalar * Time.deltaTime * 100);
    }
    
    bool OnGround {
        get {
            if (Jumping)
                return false;
            
            RaycastHit2D hit = Physics2D.Linecast(transform.position + new Vector3(-0.5f, -0.01f), transform.position + new Vector3(0.5f, -0.01f));
            if (!hit)
                return false;

            rigidbody.drag = 1.5f;
            return true;
        }
    }

    void GetJumpInput() {
        if (Jumping) {
            if (rigidbody.gravityScale < 0.9f && (Input.GetButtonUp("Jump") || (Time.time - jumpStartTime) > riseTime)) {
                // Stop rising (re-enable gravity)
                Jumping = false;
            }
        }

        // We can land and jump in the same frame
        if (!Jumping) {
            if (OnGround && Input.GetButtonDown("Jump")) {
                // Start jump
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jumpStartTime = Time.time;
                Jumping = true;
            }
        }
    }
}