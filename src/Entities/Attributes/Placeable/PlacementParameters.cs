using System;
using ZLinq;

public class PlacementParameters
{
    private readonly Func<GridCell, bool>[] _conditions;

    public PlacementParameters(params Func<GridCell, bool>[] conditions)
    {
        _conditions = conditions;
    }

    public bool CanBePlaced(GridCell cell)
    {
        return _conditions
            .AsValueEnumerable()
            .All(condition => condition?.Invoke(cell) != false);
    }
}