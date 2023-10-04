using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingController : MonoBehaviour
{
    private AligatorController aligatorController;

    private SwingWrench swingWrench;
    [SerializeField] private SwingWrench swingWrenchTemplate;

    [SerializeField] private SwingPoint activeSwingPoint;

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
                Vector2 wrenchVector = aligatorController.wrenchHolder.transform.position - activeSwingPoint.transform.position;
                float swingRadius = wrenchVector.magnitude;
                var v = characterBody.velocity.magnitude; // Transfer this velocity to be tangential to the swing
                var multiplier = 1.2f * v / swingRadius;
                Vector2 newVector = new Vector2(multiplier * wrenchVector.y,
                                                multiplier * wrenchVector.x);

                // We do some manipulation here so the player (almost) always starts going downward
                if (wrenchVector.x > 0 && wrenchVector.y > 0) {                                
                    // Sector I
                    newVector.y *= -1;
                } else if (wrenchVector.x < 0 && wrenchVector.y > 0) {
                    // Sector II
                    newVector.x *= -1;
                } else if (wrenchVector.x < 0 && wrenchVector.y < 0) {
                    // Sector III
                    newVector.x *= -1;
                } else if (wrenchVector.x > 0 && wrenchVector.y < 0) {
                    // Sector IV
                    if (characterBody.velocity.x > 0) {
                        // Special case: A player moving right and swinging from the bottom-right
                        // may expect to go upwards instead of downwards
                        newVector.x *= -1;
                    } else {
                        newVector.y *= -1;
                    }
                }
                
                characterBody.gravityScale = 0.5f;
                characterBody.constraints = RigidbodyConstraints2D.None;
                characterBody.velocity = newVector;

                // Engage new point
                activeSwingPoint.Engage(characterBody);
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
