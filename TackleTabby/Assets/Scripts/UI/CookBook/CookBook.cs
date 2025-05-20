using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CookBook : MonoBehaviour
{
    [Header("StateReferences")]
    [SerializeField]
    private ComboTracker ComboTracker;
    [SerializeField]
    private Encyclopedia Encyclopedia;
        
    [SerializeField]
    private CookBookEntry[] Entries;

    [Header("Display Rules")]
    [SerializeField]
    private CookBookSetting Settings;

    public UnityEvent OnCookBookOpen = new();
    public UnityEvent OnCookBookClose = new();
    
    private List<BaitDefinition> _matchProgress = new();

    private void Start()
    {
        ComboTracker.OnComboUpdated.AddListener(OnComboUpdated);
        ComboTracker.OnComboFinished.AddListener(OnComboFinished);
        
        foreach (CookBookEntry entry in Entries)
            entry.SetSettings(Settings);
    }

    private void OnComboUpdated(Match match)
    {
        OnCookBookOpen.Invoke();
        FishDefinition[] unlockedFish = Encyclopedia.RetrieveFishProgress().Keys.ToSmartArray();
        BaitDefinition[] unlockedBait = Encyclopedia.RetrieveBaitProgress().ToSmartArray();
        
        _matchProgress.Add(match.BaitType);

        FishDefinition[] fishes = CentralFishStorage.Instance.FindByBaitCombination(_matchProgress.ToArray());
        int entryIndex = 0;
        for (int fishIndex = 0; entryIndex < Entries.Length && fishIndex < fishes.Length; fishIndex++)
        {
            FishDefinition displayFish = fishes[fishIndex];
            
            if (_matchProgress.Count >= displayFish.RequiredBaitCombination.Length)
                continue;

            BaitDefinition displayBait = displayFish.RequiredBaitCombination[_matchProgress.Count];
            
            bool isFishLocked = !unlockedFish.Contains(displayFish);
            bool isBaitLocked = !unlockedBait.Contains(displayBait);
            
            Entries[entryIndex].DisplayWith(displayFish, displayBait, isFishLocked, isBaitLocked);
            entryIndex++;
        }
    }

    private void OnComboFinished(Combo combo)
    {
        OnCookBookClose.Invoke();
        _matchProgress.Clear();
        
        foreach (CookBookEntry entry in Entries)
            entry.Close();
    }
    
}