using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting;

public class SaveManager : MonoBehaviour
{
    [SerializeField] 
    private Encyclopedia ActiveEncyclopedia;
    [SerializeField]
    private ToggleHighlight ToggleHighlight;
    [SerializeField] 
    private HungerTracking HungerTracker;

    private string _savefileName = string.Empty;

    public string SaveFileName
    {
        get
        {
            if (_savefileName == string.Empty)
                _savefileName = Application.persistentDataPath + "/save.txt";

            return _savefileName;
        }
    }

    [ContextMenu("Saving/Save")]
    public void SaveGame()
    {
#if SAVE_ENABLED
        if (!enabled || !gameObject.activeSelf)
            return;

        int satiationAmount = HungerTracker.SaveSatiation();
        int caughtFishAmount = ActiveEncyclopedia.ReturnCaughtAmount();

        ICollection<CaughtFish> toSaveDictionary = ActiveEncyclopedia.RetrieveFishProgress().Select(
            valuePair => valuePair.Value).AsReadOnlyCollection();

        SaveInstance saveInstance = new SaveInstance()
        {
            SavedSatiationAmount = satiationAmount,
            SavedFishList = toSaveDictionary.ToArray(),
            CaughtAmount = caughtFishAmount,
            AlreadyCaughtFish = ToggleHighlight.AlreadyCaughtFish,
            AlreadyCaughtTrash = ToggleHighlight.AlreadyCaughtTrash
        };

        string saveData = JsonConvert.SerializeObject(saveInstance);

        File.WriteAllText(SaveFileName, saveData);
#endif
    }

    [ContextMenu("Loading/Load")]
    public void LoadGame()
    {
#if SAVE_ENABLED
        if (!enabled || !gameObject.activeSelf)
            return;

        if (!File.Exists(SaveFileName))
            return;

        string testText = File.ReadAllText(SaveFileName);

        if (testText == string.Empty)
            return;

        SaveInstance saveInstance = JsonConvert.DeserializeObject<SaveInstance>(testText);

        ICollection<CaughtFish> encyclopediaProgress = saveInstance.SavedFishList
            .Where(fish => fish.FishType.VerifySelf()).AsReadOnlyCollection();

        HungerTracker.LoadSatiation(saveInstance.SavedSatiationAmount);
        ToggleHighlight.SetTutorialProgress(saveInstance.AlreadyCaughtFish, saveInstance.AlreadyCaughtTrash);
        ActiveEncyclopedia.RestoreCatalogue(encyclopediaProgress);
        ActiveEncyclopedia.SetCaughtAmount(saveInstance.CaughtAmount);
#endif
    }

    [ContextMenu("Reseting/Reset")]
    private void ResetProgress()
    {
#if SAVE_ENABLED
        if (!enabled || !gameObject.activeSelf)
            return;

        File.Delete(SaveFileName);
#endif
    }
}
