using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawTrajectory : MonoBehaviour
{
    public Rigidbody trajectoryRb;

    [Header("Line Settings")]
    public int points = 50;
    public float segmentScale = .01f;
    public Color startColor = Color.green;
    public Color endColor = Color.red;
    private LineRenderer lineRenderer;

    [Header("Manual Trajectory Values")]
    public Vector3 manualVelocity;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = points;
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
        lineRenderer.widthMultiplier = 0.05f; // Adjust the width of the LineRenderer
    }

    void Update()
    {
        /*
        if (trajectoryRb == null) return;

        // Predict the trajectory
        Vector3[] segments = new Vector3[points];
        Vector3 currentPosition = trajectoryRb.position;
        Vector3 currentVelocity = trajectoryRb.velocity;

        for (int i = 0; i < points; i++)
        {
            segments[i] = currentPosition;
            currentPosition += currentVelocity * segmentScale;
            currentVelocity += Physics.gravity * segmentScale;
        }

        lineRenderer.SetPositions(segments);
        */
    }

    [ContextMenu("DrawTrajectoryOnce")]
    public void DrawTrajectoryOnce()
    {
        if (trajectoryRb == null)
        {
            Debug.Log("trajectoryRb is null");
            return;
        }

        // Predict the trajectory
        Vector3[] segments = new Vector3[points];
        Vector3 currentPosition = trajectoryRb.position;
        Vector3 currentVelocity = trajectoryRb.velocity;

        for (int i = 0; i < points; i++)
        {
            segments[i] = currentPosition;
            currentPosition += currentVelocity * segmentScale;
            currentVelocity += Physics.gravity * segmentScale;
        }

        lineRenderer.SetPositions(segments);
    }

    [ContextMenu("DrawManualTrajectory")]
    public void DrawManualTrajectory()
    {
        if (trajectoryRb == null) return;

        // Predict the trajectory
        Vector3[] segments = new Vector3[points];
        Vector3 currentPosition = trajectoryRb.position;

        for (int i = 0; i < points; i++)
        {
            segments[i] = currentPosition;
            currentPosition += manualVelocity * segmentScale;
            manualVelocity += Physics.gravity * segmentScale;
        }

        lineRenderer.SetPositions(segments);
    }

}
