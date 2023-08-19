using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AligatorController : MonoBehaviour
{
    public float maxHorizontalSpeed = 2.0f;
    public float horizontalInputScalar = 1.0f;

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
    }

    void GetHorizontalInput()
    {
        var hInput = Input.GetAxis("Horizontal");
        if ((hInput > 0 && rigidbody.velocity.x >= maxHorizontalSpeed)
            || (hInput < 0 && rigidbody.velocity.x <= -maxHorizontalSpeed)) {
            return;
        }

        rigidbody.AddForce(Vector2.right * hInput * horizontalInputScalar);
    }
}
