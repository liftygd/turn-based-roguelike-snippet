using Cysharp.Threading.Tasks;
using UnityEngine;

public class ExecutionSelectionMarker : MonoBehaviour
{
    [SerializeField] private TimelinePlayer animator;
    private EventBinding<TurnPhaseEventBinding> _turnPhaseChanged;

    private void Start()
    {
        AppearAnimation();

        _turnPhaseChanged = new EventBinding<TurnPhaseEventBinding>(OnTurnChanged);
        EventBus<TurnPhaseEventBinding>.Register(_turnPhaseChanged);
    }

    private void OnDestroy()
    {
        EventBus<TurnPhaseEventBinding>.Deregister(_turnPhaseChanged);
    }

    private UniTask OnTurnChanged(TurnPhaseEventBinding turnEvent)
    {
        if (turnEvent.TurnPhase == TurnManager.TurnPhaseType.PreparationEnd)
            DisappearAnimation();
        else if (turnEvent.TurnPhase == TurnManager.TurnPhaseType.PreparationStart)
            AppearAnimation();

        return UniTask.CompletedTask;
    }

    private void AppearAnimation()
    {
        animator.PlayAnimationAsync(new AsyncCellAnimation.ExecutionMarkerAppearAnimation()).Forget();
    }

    private void DisappearAnimation()
    {
        animator.PlayAnimationAsync(new AsyncCellAnimation.ExecutionMarkerDisappearAnimation()).Forget();
    }
}