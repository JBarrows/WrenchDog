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
        if (handleObject && holderPoint) {
            handleObject.transform.position = holderPoint.position;
        }

        if (headObject && swingPoint) {
            headObject.transform.position = swingPoint.transform.position;
        }

        // Align the shaft
        shaftLine.useWorldSpace = true;
        shaftLine.SetPositions(new Vector3[2]{handlePoint.position, headPoint.position});
    }
}
