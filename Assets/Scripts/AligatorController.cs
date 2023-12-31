using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AligatorController : MonoBehaviour
{
    public UnityEvent OnJump;
    public UnityEvent OnLand;
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
    public GameObject wrenchHolder;
    public GameObject worldWrench;
    [SerializeField] GameObject propWrench; // The wrench model held in the character's hand that doesn't do anything
    GameObject throwingWrench;
    bool isSwingDown = false;
    new Camera camera;
    bool isUsingWrench = false;
    GameObject lockedPlatform = null;
    [SerializeField] private SwingController swingController;

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
            
            // NOTE: We should use a "Floor" layer or tag instead of "Not player"
            LayerMask floorMask = ~LayerMask.GetMask("Player");
            Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.73f, 0.1f), 0f, floorMask);

            foreach (var h in hits)
            {
                if (!h.isTrigger) {
                    return true;
                } 
            }

            return false;
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
        if(swingController.IsEngaged)
        {
            Vector3 wrenchVector = swingController.ActiveSwingPoint.gameObject.transform.position - gameObject.transform.position;
            // Apply swing physics
            rigidbody.AddForce(
                wrenchVector.sqrMagnitude * Time.deltaTime *
                swingCoefficient *
                wrenchVector,
                ForceMode2D.Impulse
            );
        } else {
            GetHorizontalInput();
            GetFreezeInput();
        }

        if(rigidbody.velocityY < -0.1f && OnGround)
        {
            OnLand.Invoke();
        }

        if (Input.GetButtonDown("Reset")) {
            var sceneController = FindAnyObjectByType<SceneController>();
            if (sceneController) {
                sceneController.reload();
            }
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
                OnJump.Invoke();
            }
        }
    }

    void GetSwingInput() {
        isSwingDown = Input.GetButton("Fire2") && !isUsingWrench;
        if (!isSwingDown && swingController.IsEngaged)
        {
            // Release swing
            swingController.ActiveSwingPoint = null;
            // Show prop wrench
            SetPropWrenchActive(true);
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
                        Vector3 startPosition = gameObject.transform.position;
                        Vector3 endPosition = hit.gameObject.transform.position;
                        float distance = Vector3.Distance(startPosition, endPosition);
                        Vector3 normalizedVector = Vector3.Normalize(endPosition - startPosition);
                        if(!throwingWrench) throwingWrench = Instantiate(worldWrench, gameObject.transform.position, Quaternion.identity);
                        throwingWrench.transform.position = startPosition;
                        throwingWrench.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(normalizedVector.y, normalizedVector.x) * Mathf.Rad2Deg);
                        throwingWrench.SetActive(true);
                        throwingWrench.GetComponent<Rigidbody2D>().AddForce((endPosition - startPosition) / 0.25f, ForceMode2D.Impulse);
                        SetPropWrenchActive(false);
                        StartCoroutine(ThrowWrenchCoroutine(endPosition));
                        
                    }
                }
            } else {
                // Call wrench back
                isUsingWrench = false;
                throwingWrench.SetActive(false);
                SetPropWrenchActive(true);
                RecallWrench.Invoke();
                lockedPlatform = null;
            }
        }
    }

    IEnumerator ThrowWrenchCoroutine(Vector3 endPosition)
    {
        yield return new WaitForSeconds(0.23f);

        throwingWrench.GetComponent<Rigidbody2D>().velocity *= 0.0f;
        throwingWrench.transform.position = endPosition;

        // yield return new WaitForSeconds(0.02f);

        ThrowWrench.Invoke();
    }

    public void TogglePlatformFreeze()
    {
        lockedPlatform.BroadcastMessage("ToggleFreezePlatform");
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "SwingPoint")
        {
            if (swingController.IsEngaged)
                return;

            if (isSwingDown)
            {
                swingController.SwingOn(other.gameObject);
            }
        }
    }

    public void SetPropWrenchActive(bool value)
    {
        if (!propWrench) return;
        propWrench.SetActive(value);
    }
}
