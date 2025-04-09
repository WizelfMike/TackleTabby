using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class UpdateHungerBar : MonoBehaviour
{
    [SerializeField] 
    private Image[] HungerFishSprites;
    
    public void UpdateBar(int currentSaturation)
    {
        int hungerFishCount = HungerFishSprites.Length;
        for (int i = hungerFishCount; i > 0; i--)
        {
            HungerFishSprites[i -1].color = Color.black;
            if (i <= currentSaturation)
            {
                HungerFishSprites[i -1].color = Color.white;
            }

        }
    }
}
