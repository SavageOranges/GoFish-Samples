using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShowcaseFloat : MonoBehaviour
{
    [Header("Rotation")]
    // Rotation
    public bool EnableRotation = true;
    public float rotationSpeed = 50f;
    private Quaternion _startRot;

    [Header("Flotation")]
    // Flotation
    public bool EnableFlotation = true;
    public float floatSpeed = 2f;
    public float floatHeight = 0.1f;
    private Vector3 _startPos;

    void Start()
    {
        _startPos = transform.position;
        _startRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (EnableRotation)
        {
            // Rotate object around Y
            transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
        }
        else
        {
            transform.rotation = _startRot;
        }


        if (EnableFlotation)
        {
            // Float effect with sin wave
            float tempPos = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            transform.position = _startPos + new Vector3(0, tempPos, 0);
        }
        else
        {
            transform.position = _startPos;
        }
    }

    public void SetStartPosRot(Vector3 newStartPos, Quaternion newStartRot)
    {
        _startPos = newStartPos;
        _startRot = newStartRot;
    }
}
