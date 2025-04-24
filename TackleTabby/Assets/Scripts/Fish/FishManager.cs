using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class FishManager : MonoBehaviour
{
    [SerializeField]
    private ComboTracker ComboTracker;

    public UnityEvent<CaughtFish> OnFishCaught = new();
    public UnityEvent<TrashDefinition> OnTrashCaught = new();

    public void GetCombo(Combo combo)
    {
        BaitDefinition[] baits = combo.Entries.Select(x => x.BaitType).ToArray();
        FishDefinition[] correspondingFishes = CentralFishStorage.Instance.FindByBaitCombination(baits);
        if (correspondingFishes.Length <= 0)
        {
            TrashDefinition trash = CentralTrashStorage.Instance.GetRandomTrash();
            OnTrashCaught.Invoke(trash);
            return;
        }
        

        FishDefinition bestFish = correspondingFishes[0];
        int comboSize = combo.Entries.Aggregate(0, (total, match) => total + match.MatchSize);
        CaughtFish caughtFish = CalcFishSize(bestFish, comboSize, bestFish.SatiationAmount);
        OnFishCaught.Invoke(caughtFish);
    }

    private CaughtFish CalcFishSize(FishDefinition fishType, int comboSize, int satiationAmount)
    {
        float maxPossibleComboSize = (3f * (FieldMatchValidator.Instance.MinRequiredMatchSize - 1) + 1) * ComboTracker.RequiredComboLength;
        float minPossibleComboSize = ComboTracker.RequiredComboLength * FieldMatchValidator.Instance.MinRequiredMatchSize;
        float percentageSize = (comboSize - minPossibleComboSize) / maxPossibleComboSize;
        
        float directFishSize = Mathf.Lerp(fishType.MinSizeInches, fishType.MaxSizeInches, percentageSize);
        float noise = Random.value * fishType.MaxSizeDeviationInch;

        return new CaughtFish
        {
            FishType = new FishTypeKey(fishType),
            CaughtSize = Mathf.Min(directFishSize + noise, fishType.MaxSizeInches),
        };
    }
}
