using UnityEngine.Events;

public class MatchMediator : GenericSingleton<MatchMediator>
{
    public UnityEvent<FieldBlock, FieldBlock[]> OnMatchFound;

    public void NotifyOfMatch(FieldBlock caller, FieldBlock[] fullMatch)
    {
        OnMatchFound?.Invoke(caller, fullMatch);
    }
}