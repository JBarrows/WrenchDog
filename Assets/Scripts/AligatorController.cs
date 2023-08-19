using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AligatorController : MonoBehaviour
{
    public float maxHorizontalSpeed = 2.0f;
    public float horizontalInputScalar = 1.0f;
    public float jumpForce = 5.0f;

    int facingDirection = 1; // -1: left, 0: away/towards, 1: right
    new Rigidbody2D rigidbody;

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

        rigidbody.AddForce(Vector2.right * hInput * horizontalInputScalar);
    }

    void GetJumpInput() {
        var hInput = Input.GetButtonDown("Jump");
        if (hInput) {
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            rigidbody.gravityScale = 0.1f;
        } else if (Input.GetButtonUp("Jump")) {
            rigidbody.gravityScale = 1;
        }
    }
}
