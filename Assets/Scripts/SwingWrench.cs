using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwingWrench : MonoBehaviour
{
    public float handleOffset;
    public float headOffset;
    public SwingPoint swingPoint;
    public SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!swingPoint)
            return;

        // Rotate the Wrench to face the SwingPoint
        Vector2 wrenchVector = swingPoint.transform.position - this.transform.position;
        Quaternion r = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(wrenchVector.y, wrenchVector.x) * Mathf.Rad2Deg);
        this.transform.rotation = r;
        swingPoint.transform.rotation = r;
        var spriteWidth = Mathf.Max(0, wrenchVector.magnitude) + handleOffset + headOffset;
        sprite.transform.localPosition = Vector2.right * ((spriteWidth / 2) - handleOffset);
        sprite.size = new Vector2(spriteWidth, 1);
    }
}
