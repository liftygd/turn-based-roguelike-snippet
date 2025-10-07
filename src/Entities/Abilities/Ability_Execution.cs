using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ZLinq;

public class Ability_Execution : AbstractAbility
{
    public Ability_Execution(GridEntity owner) : base(owner)
    {
    }

    public override AbstractAbility Prepare(List<GridCell> patternCells)
    {
        var enumerable = patternCells.AsValueEnumerable();
        _affectedCells = enumerable
            .Where(cell => cell.IsOccupied() && cell.GetValue() != _ownerEntity)
            .Where(cell => cell.GetValue().GetComponent<IExecutable>() != null)
            .ToList();

        return this;
    }

    public override void ExecuteAbility()
    {
        foreach (var executable in GetUniqueEntities<IExecutable>())
            executable.Execute();
    }

    public override async UniTask ExecuteAbilityAsync()
    {
        var tasks = new List<UniTask>();
        foreach (var executable in GetUniqueEntities<IExecutable>())
            tasks.Add(executable.Execute());

        await UniTask.WhenAll(tasks);
    }
}