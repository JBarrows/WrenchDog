using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwingWrench : MonoBehaviour
{
    public GameObject handleObject;
    public Transform handlePoint;
    public GameObject headObject;
    public Transform headPoint;
    public Transform holderPoint;
    public SwingPoint swingPoint;
    public LineRenderer shaftLine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!holderPoint || !swingPoint)
            return;

        handleObject.transform.position = holderPoint.position;
        headObject.transform.position = swingPoint.transform.position;

        // Rotate the Handle to face the Head
        Vector3 wrenchVector = handleObject.transform.position - headObject.transform.position;
        Quaternion r = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(wrenchVector.y, wrenchVector.x) * Mathf.Rad2Deg);
        handleObject.transform.rotation = r;
        headObject.transform.rotation = r;
        swingPoint.transform.rotation = r;

        // Align the shaft
        shaftLine.useWorldSpace = true;
        shaftLine.SetPositions(new Vector3[2]{handlePoint.position, headPoint.position});
    }
}
