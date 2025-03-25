using System;
using UnityEngine;

[Serializable]
public struct ComboEntry
{
    public BaitDefinition BaitType;
}

[Serializable]
public struct Combo
{
    [SerializeField]
    public ComboEntry[] Entries;
}
