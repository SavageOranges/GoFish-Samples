using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.PoseDetection;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawCastLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Transform lureTipTransform;

    [Header("Line Settings")]
    public int linePointCounts = 20;
    public float castingCurveOffset = 1.0f;
    public float bobbingCurveOffset = -1.0f;
    private float curveOffset = 0.0f;
    private float elapsedLerpTime = 0.0f;
    public float lerpTime = 1.0f;
    public bool UseCastingCurveOffset = true;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;

        // Set line appearance
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.white;

        // To-Do: Singleton up / tag this
        lureTipTransform = GameObject.Find("LureTip").GetComponent<Transform>();
    }

    private void Update()
    {
        // To-Do: move the below to a coroutine
        // UseCastingCurveOffset should be true when casting rod
        if (UseCastingCurveOffset)
        {
            curveOffset = castingCurveOffset;
        }
        // Switch to bobbingCurveOffset when lure enters water, lerp midpoint values
        else
        {
            if (elapsedLerpTime < lerpTime)
            {
                curveOffset = Mathf.Lerp(castingCurveOffset, bobbingCurveOffset, elapsedLerpTime / lerpTime);
                elapsedLerpTime += Time.deltaTime;
            }
        }

        RenderLine(transform.position, lureTipTransform.position);
    }

    // Set start and end points of the line
    public void RenderLine(Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 controlPoint = CalculateMidpoint(startPosition, endPosition);

        Vector3[] positions = new Vector3[linePointCounts + 1];
        for (int i = 0; i <= linePointCounts; i++)
        {
            float t = i / (float)linePointCounts;
            positions[i] = CalculateQuadraticBezierPoint(t, startPosition, controlPoint, endPosition);
        }

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    // Use setter instead
    public void ToggleCurveBool(bool newState)
    {
        UseCastingCurveOffset = newState;
    }

    #region LinePointCalculations
    // Calculates midpoint of 2 Vector3's, maybe move to a static helper?
    Vector3 CalculateMidpoint(Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 midpoint = (startPosition + endPosition) / 2.0f;
        midpoint.y += curveOffset;
        return midpoint;
    }

    // Calculates quadratic bezier points, maybe move to a static helper?
    Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }
    #endregion
}
