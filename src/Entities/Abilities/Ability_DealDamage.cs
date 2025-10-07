using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ZLinq;

public class Ability_DealDamage : AbstractAbility
{
    private int _damage;

    public Ability_DealDamage(GridEntity owner) : base(owner)
    {
    }

    public override AbstractAbility Prepare(List<GridCell> patternCells)
    {
        var enumerable = patternCells.AsValueEnumerable();
        _affectedCells = enumerable
            .Where(cell => cell.IsOccupied() && cell.GetValue() != _ownerEntity)
            .Where(cell => cell.GetValue().GetComponent<Attribute_Health>())
            .ToList();

        return this;
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    public override void ExecuteAbility()
    {
        foreach (var entityHealth in GetUniqueEntities<Attribute_Health>())
            entityHealth.Damage(_damage);
    }

    public override UniTask ExecuteAbilityAsync()
    {
        ExecuteAbility();
        return UniTask.CompletedTask;
    }
}