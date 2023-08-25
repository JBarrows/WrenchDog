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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            indicatorCircle.color = indicatorInRangeColor;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            indicatorCircle.color = indicatorDefaultColor;
        }
    }
}
