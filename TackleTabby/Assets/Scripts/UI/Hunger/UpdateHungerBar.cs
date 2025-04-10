using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class UpdateHungerBar : MonoBehaviour
{
    [SerializeField] 
    private Image[] HungerFishImages;

    [SerializeField] 
    private Sprite SatiatedSprite;
    [SerializeField] 
    private Sprite HungerSprite;
    
    public void UpdateBar(int currentSaturation)
    {
        int hungerFishCount = HungerFishImages.Length;
        for (int i = hungerFishCount; i > 0; i--)
        {
            HungerFishImages[i -1].sprite = HungerSprite;
            if (i <= currentSaturation)
            {
                HungerFishImages[i -1].sprite = SatiatedSprite;
            }
        }
    }
}
