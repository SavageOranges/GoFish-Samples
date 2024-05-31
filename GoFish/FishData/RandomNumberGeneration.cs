using System;

public static class RandomNumberGeneration
{
    //private static Random random;
    private static readonly Random random = new Random();

    /*
    public static void Initialize(int seed)
    {
        random = new Random(seed);
    }
    */

    public static float RandomFloat(float minValue, float maxValue)
    {
        // Check that minValue is less than maxValue
        if (minValue >= maxValue)
        {
            throw new ArgumentException("minValue must be less than maxValue");
        }

        // Generate a random double value between 0.0 (inclusive) and 1.0 (exclusive)
        double randomDouble = random.NextDouble();

        // Scale and shift the random double value to fit within the range [minValue, maxValue]
        float result = (float)(randomDouble * (maxValue - minValue) + minValue);

        return result;
    }

    public static int RandomInt(int minValue, int maxValue)
    {
        if (minValue >= maxValue)
        {
            throw new ArgumentException("minValue must be less than maxValue.");
        }

        // Generate a random integer value between minValue (inclusive) and maxValue (exclusive)
        int result = random.Next(minValue, maxValue);

        return result;
    }
}
