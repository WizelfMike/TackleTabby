using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FishManager : MonoBehaviour
{
    [SerializeField]
    private Image FishImage;
    
    private Combo _receivedCombo;

    public void GetCombo(Combo combo)
    {
        _receivedCombo = combo;
        BaitDefinition[] baits = combo.Entries.Select(x => x.BaitType).ToArray();
        FishDefinition[] correspondingFishes = CentralFishStorage.Instance.FindByBaitCombination(baits);
        if (correspondingFishes.Length <= 0)
        {
            Debug.Log("Trash");
            return;
        }

        FishDefinition bestFish = correspondingFishes[0];

        Debug.Log(bestFish.DisplayName);
        FishImage.sprite = bestFish.FishSprite;
    }
}
