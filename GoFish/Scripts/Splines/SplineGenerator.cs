using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineGenerator : MonoBehaviour
{
    public SplineContainer splineContainer;

    [Header("Spline Transforms")]
    public Transform startPoint;
    public Transform endPoint;

    private void Start()
    {
        //startPoint = GameObject.FindGameObjectWithTag("FishingLure").GetComponent<Transform>();
    }

    [ContextMenu("GenerateThreePointSplineCurve")]
    public void GenerateThreePointSplineCurve()
    {
        // Assuming we're instantiating a new lure with each cast, 
        // grab a ref before generating the spline :)
        startPoint = GameObject.FindGameObjectWithTag("FishingLure").GetComponent<Transform>();

        // Turn start and end points into knots
        var startKnot = new BezierKnot
        {
            Position = startPoint.position,
            Rotation = Quaternion.identity
        };

        var endKnot = new BezierKnot
        {
            Position = endPoint.position,
            Rotation = Quaternion.identity
        };

        // Calculate midpoint
        var midKnot = new BezierKnot
        {
            Position = GetMidpoint(startPoint.position, endPoint.position),
            Rotation = Quaternion.identity
        };

        Spline spline = new Spline();
        spline.Add(new BezierKnot(startPoint.position), TangentMode.AutoSmooth);
        spline.Add(new BezierKnot(GetMidpoint(startPoint.position, endPoint.position)), TangentMode.AutoSmooth);
        spline.Add(new BezierKnot(endPoint.position), TangentMode.AutoSmooth);

        // Offset midpoint Y

        // Clear all splineContainer splines
        for (int i = 0; i < splineContainer.Splines.Count; i++)
        {
            splineContainer.RemoveSpline(splineContainer.Splines[i]);
        }

        // Add newly generated spline curve to splineContainer
        splineContainer.AddSpline(spline);
    }

    public static Vector3 GetMidpoint(Vector3 startPoint, Vector3 endPoint)
    {
        Vector3 midPoint = (startPoint + endPoint) / 2;
        midPoint.y = 6;
        return midPoint;
    }
}
