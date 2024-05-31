using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class SplineAnimateDebug : MonoBehaviour
{
    public SplineContainer splineContainer;
    public SplineAnimate splineAnimate;
    public Transform target;

    public bool HasInvokedOnSplineAnimateComplete = false;
    public UnityEvent OnSplineAnimateComplete;

    public void Update()
    {
        if (splineAnimate.ElapsedTime >= splineAnimate.Duration)
        {
            //Debug.Log("SplineAnimate has finished.");

            if (!HasInvokedOnSplineAnimateComplete)
            {
                OnSplineAnimateComplete?.Invoke();
                HasInvokedOnSplineAnimateComplete = true;
            }
        }
    }

    [ContextMenu("Play Animation")]
    public void PlayAnimation()
    {
        splineAnimate.Play();
    }

    public void Reset()
    {
        //splineAnimate.RecalculateAnimationParameters();
        splineAnimate.Restart(false);
        //RefreshProgressFields();
    }

    [ContextMenu("SetTransform")]
    public void SetTransform()
    {
        transform.position = target.transform.position;
    }

}
