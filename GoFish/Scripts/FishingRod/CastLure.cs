using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CastLure : MonoBehaviour
{
    [Header("Verlet Physics Parmeters")]
    public LineRenderer verletFishingLine;
    public GameObject verletPhysicsLure;
    [Tooltip("The Rigidbody component on the object determining casting conditions. Best results so far with a kinematic rigidbody at the rod tip.")]
    public Rigidbody verletLureRb;

    [Header("Instantiated Lure Parameters")]
    public GameObject castLurePrefab;
    [SerializeField] private GameObject castLure;
    public bool LureInstantiated = false;
    private Vector3 physicsTrackerVelocity;
    private Vector3 manualVelocityPreviousPosition;
    private Vector3 instantiatedLureVelocity;


    [Header("Casting Conditions")]
    /*
        minCastingThreshold and maxCastingThreshold are based off values seen in zRotCurrent (inspector), which is currentRotation's Z transform value.
        Almost definitely need to change this to a trigger volume or something later on, because this setup is 1) hacky and 2) needs all potential future
        rods to have the same pivot and centers (which they probably would)

        But its not great!
    */
    public float minCastingThreshold = 0f; // Minimum Z rotation value on the fishing rod to activate fishing mechanic
    public float maxCastingThreshold = 0f; // Maximum rotation value on the fishing rod to activate fishing mechanic
    public float targetCastingVelocity = 10f;
    public float lureVelocity = 0f;
    //public Quaternion currentRotation;
    private float zRotCurrent = 0f;

    [Header("Events")]
    public UnityEvent OnRotationThresholdReached;
    public UnityEvent OnTargetVelocityReached;

    // Track rotation threshold changes
    public event Action OnRotationThresholdExited;
    [SerializeField] private bool inTargetRotation = false;

    [Header("Debug")]
    public bool VisualiseLureTrajectory = false;
    public GameObject trajectoryVisualiserPrefab;
    public DrawTrajectory drawTrajectory;

    public bool InTargetRotation
    {
        get { return inTargetRotation; }
        set
        {
            if (inTargetRotation == value) return;

            // Check if the value is changing from true to false
            if (inTargetRotation && !value)
            {
                OnRotationThresholdExited?.Invoke();
            }

            inTargetRotation = value;
        }
    }

    void FixedUpdate()
    {
        // Manually calculate and store the 
        physicsTrackerVelocity = (verletLureRb.transform.position - manualVelocityPreviousPosition) / Time.fixedDeltaTime;
        manualVelocityPreviousPosition = verletLureRb.transform.position;

        // Track lureRb velocity
        //lureVelocity = ((verletLureRb.transform.position - manualVelocityPreviousPosition) / Time.deltaTime).magnitude;

        // Compare current rotation with previous rotation
        zRotCurrent = transform.rotation.eulerAngles.z;

        // Invoke OnRotationThresholdReached event if at the casting threshold
        if (zRotCurrent >= minCastingThreshold && zRotCurrent <= maxCastingThreshold) // weird calc because its negative
        {
            InTargetRotation = true;
            OnRotationThresholdReached?.Invoke();
        }
        else
        {
            InTargetRotation = false;
        }
    }

    [ContextMenu("InstantiateFreeLure")]
    public void InstantiateFreeLure()
    {
        // Don't allow casting if a Lure has already been instantiated
        if (LureInstantiated) return;

        // Get the position of the verletLureRb Rigidbody
        Vector3 lureSpawnPosition = verletLureRb.transform.position;
        Quaternion lureSpawnRotation = verletLureRb.transform.rotation;

        // Instantiate new lure at the verletLureRb position
        castLure = Instantiate(castLurePrefab, lureSpawnPosition, lureSpawnRotation);

        // Get the castLure Rigidbody
        Rigidbody castLureRb = castLure.GetComponent<Rigidbody>();

        // Apply physicsTrackerVelocity to castLureRb
        castLureRb.velocity = physicsTrackerVelocity;

        // Set LureInstantiated to prevent re-casting without reeling / resetting
        LureInstantiated = true;

        Debug.Log("Lure spawn position: " + lureSpawnPosition);
        Debug.Log("PhysicsTracker velocity: " + physicsTrackerVelocity);
        Debug.Log("FreeLureRb velocity: " + castLureRb.velocity);

        if (VisualiseLureTrajectory)
        {
            //drawTrajectory.trajectoryRb = castLureRb;
            //drawTrajectory.DrawTrajectoryOnce();

            Debug.Log("Visualising Lure Trajectory");
            GameObject trajectoryVisualiser = Instantiate(trajectoryVisualiserPrefab, lureSpawnPosition, lureSpawnRotation);
            trajectoryVisualiser.GetComponent<DrawTrajectory>().trajectoryRb = castLureRb;
            trajectoryVisualiser.GetComponent<DrawTrajectory>().DrawTrajectoryOnce();
        }

    }

    [ContextMenu("ResetCastingAbility()")]
    public void ResetCastingAbility()
    {
        // Freeze / Unfreeze verletLureRb
        verletLureRb.constraints = RigidbodyConstraints.FreezeAll;
        verletLureRb.constraints = RigidbodyConstraints.None;

        // Destroy instantiated lure
        Destroy(castLure);

        // Re-enable LineRenderer and Lure for Verlet FishingLine, Physics
        verletFishingLine.enabled = true;
        verletPhysicsLure.SetActive(true);

        // Reset the LureInstantiated bool
        LureInstantiated = false;
    }

    #region Event Subscribers
    private void OnEnable()
    {
        OnRotationThresholdExited += HandleLeavingRotationThreshold;
    }

    private void OnDisable()
    {
        OnRotationThresholdExited -= HandleLeavingRotationThreshold;
    }
    #endregion

    #region Event Actions
    private void HandleLeavingRotationThreshold()
    {
        Debug.Log("The bool changed from true to false!");

        // Store the lure velocity when it leaves the rotation threshold
        Debug.Log("LureVelocity: " + lureVelocity);
        instantiatedLureVelocity = physicsTrackerVelocity;

        OnTargetVelocityReached?.Invoke();
    }
    #endregion
}