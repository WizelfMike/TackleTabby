using UnityEngine;

public class FieldBlockPool : ObjectPool<FieldBlock>
{
    [SerializeField]
    private FieldBlock BaseTemplate;
    
    protected override FieldBlock GetFromBaseTemplate()
    {
        return Instantiate(BaseTemplate);
    }
}