using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple script to cycle through child objects at a given interval.
/// Can be set to loop or not.
/// Use with the ShowcaseFloat.cs script to create a nice showcase effect for recordings.
/// </summary>

public class ShowcaseCycleChildren : MonoBehaviour
{
    [Header("Display Options")]
    public float displayTime = 1.0f;
    private float currentDisplayTime = 0.0f;
    public bool loopCycle = true;

    public GameObject[] showcaseChildren;

    void Start()
    {
        SetupShowcaseChildren();
        StartCycling();
    }

    void SetupShowcaseChildren()
    {
        GameObject[] temp = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            temp[i] = transform.GetChild(i).gameObject;
        }

        showcaseChildren = temp;
    }

    void StartCycling()
    {
        StartCoroutine(CycleChildren());
    }

    IEnumerator CycleChildren()
    {
        for (int i = 0; i < showcaseChildren.Length; i++)
        {
            // Activate showcase child i
            showcaseChildren[i].SetActive(true);

            // Run a timer
            while (currentDisplayTime < displayTime)
            {
                currentDisplayTime += Time.deltaTime;
                yield return null;
            }

            // Deactivate showcase child i
            showcaseChildren[i].SetActive(false);

            // Reset timer after it reaches displayTime
            currentDisplayTime = 0.0f;

            if (i == showcaseChildren.Length - 1 && loopCycle)
            {
                // Reset to -1 (for loop will tick it up to 0)
                i = -1;

            }
            else if (i == showcaseChildren.Length - 1 && !loopCycle)
            {
                // End cycle
                Debug.Log("Showcase cycle complete.");
            }
        }
    }
}
