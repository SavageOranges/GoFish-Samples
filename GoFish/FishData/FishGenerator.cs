using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class FishGenerator : MonoBehaviour
{
    [Header("Seed")]
    public int seed;

    public AvailableFish availableFish;
    public FishData fishData;

    private Fish generatedFish;

    public Fish GeneratedFish
    {
        get { return generatedFish; }
    }

    [Header("Dev Menu")]
    public UnityEngine.UI.Image fishImage;
    public TextMeshProUGUI fishNameTmp;
    public TextMeshProUGUI fishLengthTmp;
    public TextMeshProUGUI fishWeightTmp;

    [ContextMenu("Generate Fish Object")]
    public void GenerateFishObject()
    {
        // Get a random FishData object from the availableFish list
        int fishIndex = RandomNumberGeneration.RandomInt(0, availableFish.availableFishData.Length);
        FishData fishObject = availableFish.availableFishData[fishIndex];

        generatedFish = new Fish()
        {
            name = fishObject.fishName,
            image = fishObject.fishSprite,
            length = GenerateFishLength(fishObject),
            weight = GenerateFishWeight(fishObject)
        };

        // Display the generated fish data
        Debug.Log("Generated Fish Object:");
        Debug.Log("Name: " + generatedFish.name);
        Debug.Log("Image: " + generatedFish.image);
        Debug.Log("Length: " + generatedFish.length + "cm");
        Debug.Log("Weight: " + generatedFish.weight + "kg");
    }

    public static float GenerateFishLength(FishData fishData)
    {
        // Get a random value between the fishData's min and max length
        float randomFishLength = RandomNumberGeneration.RandomFloat(fishData.minLength, fishData.maxLength);
        return (float)Math.Round(randomFishLength, 2);
    }

    public static float GenerateFishWeight(FishData fishData)
    {
        // Get a random value between the fishData's min and max length
        float randomFishWeight = RandomNumberGeneration.RandomFloat(fishData.minWeight, fishData.maxWeight);
        return (float)Math.Round(randomFishWeight, 2); ;
    }

    #region Dev
    public void PopulateDevMenu()
    {
        fishImage.sprite = generatedFish.image;
        fishNameTmp.text = generatedFish.name;
        fishLengthTmp.text = generatedFish.length.ToString() + "cm";
        fishWeightTmp.text = GeneratedFish.weight.ToString() + "kg";
    }
    #endregion
}

public class Fish
{
    public string name;
    public Sprite image;
    public float length;
    public float weight;
}