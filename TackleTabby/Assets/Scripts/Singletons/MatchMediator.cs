using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Events;

public class MatchMediator : GenericSingleton<MatchMediator>
{
    public UnityEvent<ICollection<FieldBlock>> OnMatchFound;

    public void NotifyOfMatch(FieldBlock caller, IEnumerable<FieldBlock> fullMatch)
    {
        OnMatchFound?.Invoke(fullMatch.Append(caller).AsReadOnlyCollection());
    }
}