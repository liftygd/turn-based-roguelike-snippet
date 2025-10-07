using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using UnityEngine;

[RequireComponent(typeof(Attribute_Health))]
public class BaseExecutable : GridEntity, IExecutable, ITurnPhaseListener, ITag_PlayerEntity
{
    protected bool _executed;
    private PlayerDeck _playerDeck;
    private PlayerInteraction _playerInteraction;
    private EventBinding<TurnPhaseEventBinding> _turnPhaseChanged;

    [ContextMenu("Execute")]
    public virtual async UniTask Execute()
    {
        if (!isPlaced) return;
        if (_executed) return;

        _executed = true;
        CellExecutionAnimation();

        EventBus<ExecutableEventBinding>.Raise(new ExecutableEventBinding
        {
            Executable = this,
            ExecType = ExecutableEventBinding.ExecutionType.StartedExecuting
        });

        await Execution();

        EventBus<ExecutableEventBinding>.Raise(new ExecutableEventBinding
        {
            Executable = this,
            ExecType = ExecutableEventBinding.ExecutionType.StoppedExecuting
        });
    }

    public void Reset()
    {
        _executed = false;
    }

    public async UniTask OnTurnChanged(TurnPhaseEventBinding turnEvent)
    {
        if (turnEvent.TurnPhase == TurnManager.TurnPhaseType.PreparationStart)
            _executed = false;

        if (turnEvent.TurnPhase == TurnManager.TurnPhaseType.ExecutionEnd
            && _playerInteraction.ConditionsAreMet(PlayerInteractionGroup.GroupType.DuringPlayerTurn))
            Reset();
    }

    [Inject]
    public void Construct(PlayerInteraction playerInteraction, PlayerDeck playerDeck)
    {
        _playerInteraction = playerInteraction;
        _playerDeck = playerDeck;
    }

    protected override void Configure()
    {
        base.Configure();

        _turnPhaseChanged = new EventBinding<TurnPhaseEventBinding>(OnTurnChanged);
        EventBus<TurnPhaseEventBinding>.Register(_turnPhaseChanged);
    }

    protected override void Dispose()
    {
        EventBus<TurnPhaseEventBinding>.Deregister(_turnPhaseChanged);

        EventBus<ExecutableEventBinding>.Raise(new ExecutableEventBinding
        {
            Executable = this,
            ExecType = ExecutableEventBinding.ExecutionType.Destroyed
        });

        if (!Data.TryGetData(out ExecutableData data)) return;
        if (data.characteristics.Contains(EntityCharacteristics.Clone)) return;
        if (!data.characteristics.Contains(EntityCharacteristics.Renewable)) return;

        _playerDeck.ReturnToPlayingDeck(data);
    }

    protected void CellExecutionAnimation()
    {
        foreach (var offset in occupiedCells)
            _computerGrid.GetCell(gridPosition.x + offset.x, gridPosition.y + offset.y)
                .animator.Animate(GridCellVisualLayer.Foreground, new AsyncCellAnimation.ExecutionAnimation());
    }

    protected virtual UniTask Execution()
    {
        return UniTask.CompletedTask;
    }
}