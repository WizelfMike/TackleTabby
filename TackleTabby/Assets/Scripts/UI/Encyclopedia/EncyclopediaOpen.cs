using UnityEngine;

public class EncyclopediaOpen : MonoBehaviour
{
    [SerializeField]
    private GameObject Encyclopedia;

    private bool Clickphase;
    public void OnPushedButton()
    {
        Clickphase = !Clickphase;
        Encyclopedia.SetActive(Clickphase);
        print("Pushed");
    }
}
