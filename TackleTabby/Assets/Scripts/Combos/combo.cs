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
    public ComboEntry[] Entries;
}
