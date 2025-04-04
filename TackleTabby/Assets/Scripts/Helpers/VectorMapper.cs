using System;
using UnityEngine;

[Serializable]
public struct VectorMapper
{
    [SerializeField]
    public Vector2[] MappedVectors;

    public Vector2 MapInput(Vector2 input)
    {
        input.Normalize();
        
        float bestValue = -1;
        int bestIndex = 0;

        int mappedCount = MappedVectors.Length;
        for (int i = 0; i < mappedCount; i++)
        {
            Vector2 currentMappedValue = MappedVectors[i];
            float currentBestValue = Vector2.Dot(input, currentMappedValue);

            if (currentBestValue > bestValue)
            {
                bestValue = currentBestValue;
                bestIndex = i;
            }

            if (Mathf.Abs(bestValue - 1) < Mathf.Epsilon)
                break;
        }

        return MappedVectors[bestIndex];
    }
}
