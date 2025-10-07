using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using UnityEngine;

public class Executable_Bomb : BaseExecutable
{
    [Header("Custom")] [SerializeField] private PatternData patternData;

    private Ability_AnimateCells _animateCells;
    private Attribute_Health _attributeHealth;
    private Ability_DealDamage _damageAbility;
    private Ability_Execution _executionAbility;

    private PlayerDamageModificationController _playerDamageModificationController;

    [Inject]
    public void Construct(PlayerDamageModificationController playerDamageModificationController)
    {
        _playerDamageModificationController = playerDamageModificationController;
    }

    protected override void Configure()
    {
        base.Configure();

        _playerDamageModificationController.ApplyModifiers(this, this, Data.BaseStats.GetMediator());

        _attributeHealth = gameObject.GetComponent<Attribute_Health>();
        _attributeHealth.OnDeath.asyncEvent += Execute;
        asyncAnimator.triggers.RegisterTrigger(TimelineTriggers.TriggerType.TriggerEnter, "Explosion", Explode);

        _animateCells = new Ability_AnimateCells(this, _computerGrid);
        _damageAbility = new Ability_DealDamage(this);
        _executionAbility = new Ability_Execution(this);
    }

    protected override void Dispose()
    {
        base.Dispose();

        asyncAnimator.triggers.DeregisterTrigger(TimelineTriggers.TriggerType.TriggerEnter, "Explosion", Explode);
    }

    protected override async UniTask Execution()
    {
        await asyncAnimator.PlayAnimationAsync(new AsyncAnimation.StartExecutionAnimation());
    }

    private void Explode()
    {
        var patternCells = patternData.GetCellsFromPattern(_computerGrid, gridPosition);
        _damageAbility.SetDamage(Data.BaseStats.GetStat(StatType.Attack));
        _damageAbility.Prepare(patternCells).FilterFor<ITag_EnemyEntity>().ExecuteAbility();

        _animateCells.SetAnimationData(new AsyncCellAnimation.ExplodeAnimation(), GridCellVisualLayer.Background);
        _animateCells.Prepare(patternCells).ExecuteAbility();

        _executionAbility.Prepare(patternCells).FilterFor<ITag_PlayerEntity>().ExecuteAbility();
        _attributeHealth.Damage(_attributeHealth.currentHealth);
    }
}