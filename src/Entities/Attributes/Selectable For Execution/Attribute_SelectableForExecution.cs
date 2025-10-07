using Reflex.Attributes;
using UnityEngine.InputSystem;

public class Attribute_SelectableForExecution : BaseAttribute
{
    private BaseExecutable _executable;
    private int _executionIndex = -1;

    private ExecutionSelectionManager _executionSelectionManager;
    private PlayerInteraction _playerInteraction;

    private void OnDestroy()
    {
        _executionSelectionManager.RemoveFromExecutionSelection(_executionIndex, _executable);
    }

    private void OnMouseOver()
    {
        if (!_playerInteraction.ConditionsAreMet(
                PlayerInteractionGroup.GroupType.DuringPlayerTurn,
                PlayerInteractionGroup.GroupType.WhenNotExecuting,
                PlayerInteractionGroup.GroupType.WhenHandIsEmpty
            )) return;

        if (!Mouse.current.rightButton.wasPressedThisFrame) return;

        ChangeExecutionState();
    }

    [Inject]
    public void Construct(ExecutionSelectionManager executionSelectionManager, PlayerInteraction playerInteraction)
    {
        _executionSelectionManager = executionSelectionManager;
        _playerInteraction = playerInteraction;
    }

    protected override void Configure()
    {
        _executable = _ownerEntity as BaseExecutable;
        if (_executable == null)
            enabled = false;
    }

    private void ChangeExecutionState()
    {
        _executionIndex = _executionSelectionManager.CheckIndex(_executable);

        if (_executionIndex == -1)
        {
            _executionIndex = _executionSelectionManager.AddToExecutionSelection(_executable);
        }
        else
        {
            _executionSelectionManager.RemoveFromExecutionSelection(_executionIndex, _executable);
            _executionIndex = -1;
        }
    }
}