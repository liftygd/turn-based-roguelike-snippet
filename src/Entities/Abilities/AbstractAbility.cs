using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ZLinq;

public abstract class AbstractAbility
{
    protected List<GridCell> _affectedCells;
    protected GridEntity _ownerEntity;

    public AbstractAbility(GridEntity owner)
    {
        _ownerEntity = owner;
    }

    public List<GridCell> GetCells()
    {
        return _affectedCells;
    }

    public abstract AbstractAbility Prepare(List<GridCell> patternCells);

    public AbstractAbility FilterOut<T>()
    {
        var enumerable = _affectedCells.AsValueEnumerable();
        _affectedCells = enumerable
            .Except(enumerable.Where(cell => cell.GetValue() != null && cell.GetValue().GetComponent<T>() != null))
            .ToList();

        return this;
    }

    public AbstractAbility FilterFor<T>()
    {
        var enumerable = _affectedCells.AsValueEnumerable();
        _affectedCells = enumerable
            .Except(enumerable.Where(cell => cell.GetValue() == null || cell.GetValue().GetComponent<T>() == null))
            .ToList();

        return this;
    }

    protected void Debug_ColorCells(Color color)
    {
        foreach (var cell in _affectedCells)
            cell.highlight.Highlight(GridCellVisualLayer.Foreground, color);
    }

    protected void Debug_ClearCells()
    {
        foreach (var cell in _affectedCells)
            cell.highlight.ClearAll();
    }

    protected IEnumerable<T> GetUniqueEntities<T>()
    {
        var newList = new List<T>();

        foreach (var cell in _affectedCells)
        {
            if (cell.GetValue() == null) continue;

            var entity = cell.GetValue().GetComponent<T>();
            if (entity == null) continue;
            if (newList.Contains(entity)) continue;

            newList.Add(entity);
            yield return entity;
        }
    }

    public abstract void ExecuteAbility();
    public abstract UniTask ExecuteAbilityAsync();
}