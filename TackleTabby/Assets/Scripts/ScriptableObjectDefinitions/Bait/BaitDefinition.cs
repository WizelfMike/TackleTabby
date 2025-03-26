using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BaitDefinition", menuName = "Scriptable Objects/BaitDefinition")]
public class BaitDefinition : ScriptableObject, IEquatable<BaitDefinition>, IComparable<BaitDefinition>
{
    public string DisplayName;
    public Sprite BaitSprite;

    public bool Equals(BaitDefinition other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return DisplayName == other.DisplayName;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((BaitDefinition) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), DisplayName);
    }

    public int CompareTo(BaitDefinition other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return string.Compare(DisplayName, other.DisplayName, StringComparison.Ordinal);
    }
}
