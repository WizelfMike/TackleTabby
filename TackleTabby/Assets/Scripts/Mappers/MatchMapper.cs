using System.Collections.Generic;
using System.Linq;

public class MatchMapper : IMapper<ICollection<FieldBlock>, Match>
{
    public Match MapFrom(ICollection<FieldBlock> obj)
    {
        return new Match
        {
            BaitType = obj.First().BaitDefinitionReference,
            MatchSize = obj.Count
        };
    }
}