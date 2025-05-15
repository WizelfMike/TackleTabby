using UnityEngine;
using UnityEngine.SceneManagement;

public class FullReset : MonoBehaviour
{
    [SerializeField]
    private SaveManager SaveManager;

    [SerializeField]
    private AudioHandler AudioHandler;

    public void ResetData()
    {
        SaveManager.ResetProgress();

        AudioHandler.EndMusicPlayback();

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
