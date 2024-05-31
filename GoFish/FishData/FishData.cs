using UnityEngine;

[CreateAssetMenu(fileName = "FishData", menuName = "XRFishing/FishData", order = 1)]
public class FishData : ScriptableObject
{
    public string fishName;
    public Sprite fishSprite;

    public float minLength;
    public float maxLength;
    public float minWeight;
    public float maxWeight;
}
