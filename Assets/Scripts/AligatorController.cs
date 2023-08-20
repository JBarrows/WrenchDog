using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AligatorController : MonoBehaviour
{
    [SerializeField] private UnityEvent ThrowWrench;
    [SerializeField] private UnityEvent RecallWrench;

    public float maxHorizontalSpeed = 6.0f;
    public float horizontalInputScalar = 5.0f;
    public float jumpForce = 10.0f;
    public float jumpSlow = 0.5f;
    public float stdGravity = 1.5f;
    public float riseTime = 0.5f; // How long gravity is suspended as you hold the jump key
    public float riseGravity = 0.3f;
    public float swingCoefficient = 1.0f;

    float swingRadius = 0.0f;
    bool isSwinging = false;
    bool isSwingDown = false;
    Vector3 swingAnchor = new Vector3(0.0f, 0.0f, 0.0f);
    Camera camera;
    bool isUsingWrench = false;
    GameObject lockedPlatform = null;

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
                rigidbody.gravityScale = riseGravity;
            } else {
                rigidbody.gravityScale = 1.5f;
            }
        }
    }

    bool OnGround
    {
        get {
            if (Jumping)
                return false;
            
            RaycastHit2D hit = Physics2D.Linecast(transform.position + new Vector3(-0.73f / 2F, -0.01f), transform.position + new Vector3(0.73f/2F, -0.01f));
            
            if (!hit) {
                return false;
            }

            return true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        GetJumpInput();
        GetSwingInput();
        if(isSwinging)
        {
            float distance = Vector3.Distance(gameObject.transform.position, swingAnchor);
            rigidbody.AddForce(
                distance * distance * distance * Time.deltaTime *
                Vector3.Normalize(swingAnchor - gameObject.transform.position) *
                swingCoefficient,
                ForceMode2D.Impulse
            );
        } else {
            GetHorizontalInput();
            GetFreezeInput();
        }
    }

    void GetHorizontalInput()
    {
        var hInput = Input.GetAxis("Horizontal");
        var maxSpeed = maxHorizontalSpeed;
        if (!OnGround)
            maxSpeed *= jumpSlow;

        if (hInput > 0) {
            // Pressing right
            if (rigidbody.velocity.x >= maxSpeed) {
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

        if (!OnGround)
            hInput *= jumpSlow;

        rigidbody.AddForce(Vector2.right * hInput * horizontalInputScalar * Time.deltaTime * 100);
    }
    
    void GetJumpInput() {
        if (Jumping) {
            if (rigidbody.gravityScale < stdGravity && (Input.GetButtonUp("Jump") || (Time.time - jumpStartTime) > riseTime)) {
                // Stop rising (re-enable gravity)
                Jumping = false;
            }
        }

        // We can land and jump in the same frame
        if (!Jumping) {
            if (OnGround && Input.GetButtonDown("Jump")) {
                // Start jump
                rigidbody.velocityX *= jumpSlow;
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jumpStartTime = Time.time;
                Jumping = true;
            }
        }
    }

    void GetSwingInput() {
        isSwingDown = Input.GetButton("Fire2") && !isUsingWrench;
        if (!isSwingDown && swingRadius > 0.0f)
        {
            swingRadius = 0.0f;
            isSwinging = false;
        }
    }

    void GetFreezeInput() {
        if(Input.GetButtonDown("Fire1"))
        {
            if(!isUsingWrench){
                Vector3 pos = camera.ScreenToWorldPoint(Input.mousePosition);
                Collider2D[] hits = Physics2D.OverlapCircleAll(new Vector2(pos.x, pos.y), 0);
                foreach(Collider2D hit in hits) {
                    if(hit.tag == "PlatformBolt")
                    {
                        isUsingWrench = true;
                        lockedPlatform = hit.gameObject.transform.parent.gameObject;
                        // play throw wrench animation
                        ThrowWrench.Invoke();
                    }
                }
            } else {
                // Call wrench back
                isUsingWrench = false;
                RecallWrench.Invoke();
                lockedPlatform = null;
            }
        }
    }

    public void TogglePlatformFreeze()
    {
        lockedPlatform.BroadcastMessage("ToggleFreezePlatform");
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "SwingPoint")
        {
            if(swingRadius <= 0.0f && isSwingDown)
            {
                swingAnchor = other.gameObject.transform.position;
                swingRadius = Vector3.Distance(gameObject.transform.position, swingAnchor);
                isSwinging = true;
            }
        }
    }
}
