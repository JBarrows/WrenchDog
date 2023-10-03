using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingController : MonoBehaviour
{
    private AligatorController aligatorController;

    private SwingWrench swingWrench;
    [SerializeField] private SwingWrench swingWrenchTemplate;

    [SerializeField] private SwingPoint activeSwingPoint;

    private float swingRadius = 0.0f;

    public SwingPoint ActiveSwingPoint
    {
        get { return activeSwingPoint; }
        
        set {
            if (value == activeSwingPoint)
                return;

            var characterBody = aligatorController.GetComponent<Rigidbody2D>();

            if (activeSwingPoint) {
                // Disengage old point
                activeSwingPoint.Disengage();

                if (swingWrench) {
                    swingWrench.gameObject.SetActive(false);
                    characterBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                    characterBody.gravityScale = aligatorController.stdGravity;
                    characterBody.transform.rotation = Quaternion.identity;
                }
            }

            activeSwingPoint = value;

            if (activeSwingPoint) {
                var v = characterBody.velocity; // Transfer this velocity to be tangential to the swing
                characterBody.gravityScale = 0.5f;
                characterBody.constraints = RigidbodyConstraints2D.None;
                // Engage new point
                activeSwingPoint.Engage(characterBody);
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
        aligatorController = GetComponent<AligatorController>();
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
        
        // Make sure we have a SwingWrench
        if (!swingWrench) { 
            swingWrench = Instantiate(swingWrenchTemplate, aligatorController.wrenchHolder.transform);
        }

        if (swingWrench) {
            // Hide Prop Wrench
            aligatorController.SetPropWrenchActive(false);

            // COnnect and show SwingWrench
            swingWrench.swingPoint = swingPoint;
            swingWrench.gameObject.SetActive(true);
        }
    }
}
