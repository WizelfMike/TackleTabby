using System.IO;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        LoadGame();
    }

    [ContextMenu("Saving/Save")]
    public void SaveGame()
    {
        int satiationAmount = HungerTracker.SaveSatiation();

        SaveInstance saveInstance = new SaveInstance()
        {
            SavedSatiationAmount = satiationAmount
        };

        string saveData = JsonUtility.ToJson(saveInstance);

        File.WriteAllText(SaveFileName, saveData);
    }

    [ContextMenu("Loading/Load")]
    private void LoadGame()
    {
        string testText = File.ReadAllText(SaveFileName);

        if (testText == string.Empty)
            return;

        SaveInstance saveInstance = JsonUtility.FromJson<SaveInstance>(testText);

        ActiveEncyclopedia.RetrieveFishProgress();

        HungerTracker.LoadSatiation(saveInstance.SavedSatiationAmount);
    }

    [ContextMenu("Reseting/Reset")]
    private void ResetProgress()
    {
        SaveInstance saveInstance = new SaveInstance()
        {
            SavedSatiationAmount = 10
        };

        string saveData = JsonUtility.ToJson(saveInstance);

        File.WriteAllText(SaveFileName, saveData);

        string testText = File.ReadAllText(SaveFileName);

        SaveInstance LoadInstance = JsonUtility.FromJson<SaveInstance>(testText);

        HungerTracker.LoadSatiation(LoadInstance.SavedSatiationAmount);

    }
}
