using UnityEngine;

public class PauzeTime : MonoBehaviour
{
    public void Pauze()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }
}
