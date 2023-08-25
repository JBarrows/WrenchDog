using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingContoller : MonoBehaviour
{
    [SerializeField] private SwingPoint activeSwingPoint;

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
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
