using UnityEngine;

public class CloudPool : ObjectPool<MovingCloud>
{
    [SerializeField]
    private MovingCloud BaseTemplate;
    
    protected override MovingCloud GetFromBaseTemplate()
    {
        return Instantiate(BaseTemplate);
    }
}