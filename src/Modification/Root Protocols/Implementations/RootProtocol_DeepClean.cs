using Reflex.Attributes;
using UnityEngine;

public class RootProtocol_DeepClean : BaseRootProtocol, IValueModifier
{
    [SerializeField] private float multiplyAmount = 1.1f;

    private PlayerDamageModificationController _playerDamageModificationController;
    public override ModifierOrder Order { get; } = ModifierOrder.Lowest;
    public override bool IsStackable { get; } = true;

    public void ApplyModifier(GridEntity entity, object caller, StatsMediator mediator)
    {
        if (entity is not ITag_PlayerEntity) return;

        mediator.AddModifier(new StatModifier(StatType.Attack, new MultiplyOperation(multiplyAmount)));
    }

    [Inject]
    public void Construct(PlayerDamageModificationController playerDamageModificationController)
    {
        _playerDamageModificationController = playerDamageModificationController;
    }

    protected override void Configure()
    {
        _playerDamageModificationController.AddModifier(this);
    }

    protected override void Dispose()
    {
        _playerDamageModificationController.RemoveModifier(this);
    }
}