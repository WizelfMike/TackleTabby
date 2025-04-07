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
}