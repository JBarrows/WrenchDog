using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    public Color indicatorDefaultColor;
    public Color indicatorInRangeColor;
    public Color indicatorGrappledColor;

    [SerializeField] private SpriteRenderer indicatorCircle;

    private bool isEngaged = false;
    private bool isInRange = false;

    public bool IsEngaged
    {
        get { return isEngaged; }
        
        set {
            Debug.Log("Engaged: " + value);
            isEngaged = value;
            DetermineColor();
        }
    }
    
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
            color = indicatorGrappledColor;
        } else if (isInRange) {
            color = indicatorInRangeColor;
        }

        indicatorCircle.color = color;
    }
}
