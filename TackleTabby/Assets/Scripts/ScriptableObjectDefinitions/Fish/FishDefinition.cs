using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FishDefinition", menuName = "Scriptable Objects/FishDefinition")]
public class FishDefinition : ScriptableObject
{
    [Header("Display")]
    public string DisplayName;
    public Sprite FishSprite;
    public Sprite ThumbnailSprite;

    [Header("Size Definition")]
    [Range(0.1f, 1000f)]
    public float MinSizeInches;
    [Range(0.1f, 1000f)]
    public float MaxSizeInches;
    public float MaxSizeDeviationInch;

    [Header("Catching")]
    public BaitDefinition[] RequiredBaitCombination;

    [Header("Satiation")] 
    [Range(1,10)]
    public int SatiationAmount;
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(DisplayName))
            Debug.LogError("Display-name may not be empty");

        if (MaxSizeInches < MinSizeInches)
        {
            MaxSizeInches = MinSizeInches;
            Debug.LogError("Max size of the fish may not be less than it's minimal size");
        }
        
        if (RequiredBaitCombination == null || RequiredBaitCombination.Length == 0)
            Debug.LogError("RequiredBaitCombo may not be empty");
    }

    public Hash128 MagicKey()
    {
        Hash128 hash = new Hash128();
        
        hash.Append(DisplayName);
        hash.Append(MinSizeInches);
        hash.Append(MaxSizeInches);
        hash.Append(MaxSizeDeviationInch);
        hash.Append(SatiationAmount);
        
        foreach (BaitDefinition bait in RequiredBaitCombination)
            hash.Append(bait.GetMagicKey().ToString());
        
        return hash;
    }
}