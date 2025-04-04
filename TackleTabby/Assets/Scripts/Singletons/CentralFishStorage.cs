using System;
using System.Collections.Generic;
using UnityEngine;

public class CentralFishStorage : GenericSingleton<CentralFishStorage>
{
    [SerializeField]
    private FishDefinition[] Fish;
    
    public ReadOnlySpan<FishDefinition> GetAllFish()
    {
        return Fish;
    }

    /// <summary>
    /// Finds the first occurrence of the fish with `fishName`
    /// </summary>
    /// <param name="fishName">The name to search for</param>
    /// <returns>The fish with the given name. When none found, returns null</returns>
    public FishDefinition FindByName(string fishName)
    {
        ReadOnlySpan<FishDefinition> readonlyArray = Fish;
        int length = readonlyArray.Length;

        for (int i = 0; i < length; i++)
        {
            FishDefinition fish = readonlyArray[i];
            if (fish.DisplayName.Equals(fishName, StringComparison.Ordinal))
                return fish;
        }

        return null;
    }

    /// <summary>
    /// Finds all the fish that have the specified bait combination
    /// </summary>
    /// <param name="baitCombination">The bait combination to search for</param>
    /// <returns>All the fish that contain the given bait combination, in order of best match first. When none could be found, returns an empty array</returns>
    public FishDefinition[] FindByBaitCombination(BaitDefinition[] baitCombination)
    {
        ReadOnlySpan<FishDefinition> readonlyArray = Fish;
        int length = readonlyArray.Length;
        List<FishDefinition> foundFishes = new();

        for (int i = 0; i < length; i++)
        {
            FishDefinition currentFish = readonlyArray[i];
            ReadOnlySpan<BaitDefinition> currentFishBaitCombination = currentFish.RequiredBaitCombination;
            int index = currentFishBaitCombination.IndexOf(baitCombination);
            if (index != -1)
                foundFishes.Add(currentFish);
        }

        foundFishes.Sort((a, b) => a.RequiredBaitCombination.Length - b.RequiredBaitCombination.Length);
        return foundFishes.ToArray();
    }
    
    #if UNITY_EDITOR

    [ContextMenu("Find/By Name (Cat Fish)")]
    private void FindOnNameCatFish()
    {
        FishDefinition catFish = FindByName("Cat Fish");
        Debug.Log(catFish.DisplayName);
    }

    [ContextMenu("Find/By Bait (shrimp)")]
    private void FindOnBaitType()
    {
        BaitDefinition myShrimpBait = ScriptableObject.CreateInstance<BaitDefinition>();
        myShrimpBait.DisplayName = "Shrimp";
        FishDefinition[] found = FindByBaitCombination(
            new[]
            {
                myShrimpBait,
                myShrimpBait
            });
        
        if (found.Length == 0)
            Debug.Log("No matches were found");
        
        foreach (FishDefinition fish in found)
            Debug.Log(fish.DisplayName);
    }
    
    #endif
}
