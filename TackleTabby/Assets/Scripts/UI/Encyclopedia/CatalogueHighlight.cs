using UnityEngine;

public class CatalogueHighlight : MonoBehaviour
{
    [SerializeField] 
    private GameObject FieldGrayout;
    [SerializeField] 
    private Encyclopedia Encyclopedia;

    [SerializeField] private MenuCommunicator MenuCommunicator;
    public void EnableHighlight()
    {
        FieldGrayout.SetActive(true);
        
    }

    public void DisableHighlight()
    {
        
    }
}
