using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DynamicPlatform : MonoBehaviour
{

    public bool isLocked = false;
    public GameObject child;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ToggleFreezePlatform()
    {
        Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
        rb.freezeRotation = !rb.freezeRotation;
    }

}
