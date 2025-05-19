using UnityEngine;

public static class PauzeTime
{
    public static void Pauze()
    {
        Time.timeScale = 0;
    }

    public static void Resume()
    {
        Time.timeScale = 1;
    }
}
