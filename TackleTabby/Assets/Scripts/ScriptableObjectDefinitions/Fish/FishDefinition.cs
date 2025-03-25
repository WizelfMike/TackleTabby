using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FishDefinition", menuName = "Scriptable Objects/FishDefinition")]
public class FishDefinition : ScriptableObject, IEquatable<FishDefinition>
{
    [Header("Display")]
    public string DisplayName;
    public Sprite FishSprite;

    [Header("Size Definition")]
    [Range(0.1f, 1000f)]
    public float MinSizeInches;
    [Range(0.1f, 1000f)]
    public float MaxSizeInches;
    public float MaxSizeDeviationInch;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(DisplayName))
            Debug.LogError("Display-name may not be empty");

        if (MaxSizeInches < MinSizeInches)
        {
            MaxSizeInches = MinSizeInches;
            Debug.LogError("Max size of the fish may not be less than it's minimal size");
        }
    }

    public bool Equals(FishDefinition other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) && DisplayName == other.DisplayName && FishSprite.Equals(other.FishSprite);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((FishDefinition) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), DisplayName, FishSprite);
    }
}