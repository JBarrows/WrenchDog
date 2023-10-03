using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwingPoint : MonoBehaviour
{
    public Color indicatorDefaultColor;
    public Color indicatorInRangeColor;
    public Color indicatorEngagedColor;

    [SerializeField] private SpriteRenderer indicatorCircle;

    [SerializeField] private HingeJoint2D m_hingeJoint;

    [SerializeField] private Rigidbody2D weightBody;

    private bool isEngaged = false;
    private bool isInRange = false;

    public bool IsEngaged
    {
        get { return isEngaged; }
        
        set {
            isEngaged = value;
            
            DetermineColor();
            
            if (weightBody) {
                // The weight adds a fun little motion while the bolt isn't engaged
                weightBody.simulated = !isEngaged;
            }
        }
    }
    
    public HingeJoint2D hingeJoint { get => m_hingeJoint; set => m_hingeJoint = value; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            isInRange = true;
            DetermineColor();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            isInRange = false;
            DetermineColor();
        }
    }

    private void DetermineColor()
    {
        Color color = indicatorDefaultColor;
        
        if (isEngaged) {
            color = indicatorEngagedColor;
        } else if (isInRange) {
            color = indicatorInRangeColor;
        }

        indicatorCircle.color = color;
    }

    public void Disengage()
    {
        hingeJoint.connectedBody = weightBody;
        IsEngaged = false;
    }

    public void Engage(Rigidbody2D connectedBody)
    {
        IsEngaged = true;
        hingeJoint.connectedBody = connectedBody;
    }
}
