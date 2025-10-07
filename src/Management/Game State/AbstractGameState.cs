using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class AbstractGameState : MonoBehaviour
{
    [SerializeField] private GameObject stateObject;
    public abstract GameStateType stateType { get; }

    private void Awake()
    {
        DeactivateStateObject();
    }

    public void ActivateStateObject()
    {
        if (stateObject == null) return;
        stateObject.SetActive(true);
    }

    public void DeactivateStateObject()
    {
        if (stateObject == null) return;
        stateObject.SetActive(false);
    }

    public abstract UniTask EnterState(GameStateManager manager);
    public abstract UniTask ExitState();
}