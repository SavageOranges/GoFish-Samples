using UnityEngine;

[CreateAssetMenu(fileName = "AvailableFish", menuName = "XRFishing/AvailableFish", order = 2)]
public class AvailableFish : ScriptableObject
{
    public FishData[] availableFishData;
}
