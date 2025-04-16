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
        if (!enabled)
            return;

        int satiationAmount = HungerTracker.SaveSatiation();

        ICollection<CaughtFish> toSaveDictionary = ActiveEncyclopedia.RetrieveFishProgress().Select(
            valuePair => valuePair.Value).AsReadOnlyCollection();

        SaveInstance saveInstance = new SaveInstance()
        {
            SavedSatiationAmount = satiationAmount,
            SavedFishList = toSaveDictionary.ToArray()
        };

        string saveData = JsonConvert.SerializeObject(saveInstance);

        File.WriteAllText(SaveFileName, saveData);
    }

    [ContextMenu("Loading/Load")]
    public void LoadGame()
    {
        if (!enabled)
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
        ActiveEncyclopedia.RestoreCatalogue(encyclopediaProgress);
    }

    [ContextMenu("Reseting/Reset")]
    private void ResetProgress()
    {
        if (!enabled)
            return;

        File.Delete(SaveFileName);
    }
}
