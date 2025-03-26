
using System.Collections.Generic;
using UnityEngine;

public class FieldMatchValidator : GenericSingleton<FieldMatchValidator>
{
    [SerializeField]
    [Range(1, 10)]
    private int MinimalRequiredMatchSize = 3;

    public bool ValidateMatch(FieldBlock caller, IReadOnlyList<FieldBlock> match)
    {
        int size = match.Count;
        
        if (size < MinimalRequiredMatchSize - 1)
            return false;

        BaitDefinition callerType = caller.BaitDefinitionReference;
        for (int i = 0; i < size; i++)
        {
            if (!match[i].BaitDefinitionReference.Equals(callerType))
                return false;
        }

        return true;
    }
}