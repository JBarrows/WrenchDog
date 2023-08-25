using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingController : MonoBehaviour
{
    [SerializeField] private SwingPoint activeSwingPoint;

    private float swingRadius = 0.0f;

    public SwingPoint ActiveSwingPoint
    {
        get { return activeSwingPoint; }
        
        set {
            if (value == activeSwingPoint)
                return;

            if (activeSwingPoint) {
                // Disengage old point
                activeSwingPoint.IsEngaged = false;
            }

            activeSwingPoint = value;

            if (activeSwingPoint) {
                // Engage new point
                activeSwingPoint.IsEngaged = true;
                swingRadius = Vector3.Distance(gameObject.transform.position, activeSwingPoint.gameObject.transform.position);
            }
        }
    }

    public bool IsEngaged { 
        get { return activeSwingPoint != null; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwingOn(GameObject target)
    {
        var swingPoint = target.GetComponent<SwingPoint>();
        if (!swingPoint)
            return;
        
        ActiveSwingPoint = swingPoint;
    }
}
