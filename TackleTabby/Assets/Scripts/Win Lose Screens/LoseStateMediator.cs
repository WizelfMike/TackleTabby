using UnityEngine;
using UnityEngine.Events;

public class LoseStateMediator : MonoBehaviour
{
    [SerializeField]
    private WinLoseBehaviour LosePopUp;
    [SerializeField]
    private GameObject[] ToBeDisabled;

    public UnityEvent LoseEvent;

    public void OnLost()
    {
        LoseEvent?.Invoke();

        for (int i = 0; i < ToBeDisabled.Length; i++)
        {
            ToBeDisabled[i].SetActive(false);
        }
    }
}
