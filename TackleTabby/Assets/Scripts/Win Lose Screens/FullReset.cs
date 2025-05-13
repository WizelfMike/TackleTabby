using UnityEngine;
using UnityEngine.SceneManagement;

public class FullReset : MonoBehaviour
{
    [SerializeField]
    private SaveManager SaveManager;

    public void ResetData()
    {
        SaveManager.ResetProgress();

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
