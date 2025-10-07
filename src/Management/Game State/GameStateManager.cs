using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public enum GameStateType
{
    Configuration,
    Fight,
    FightSummary,
    Directory
}

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private TimelinePlayer loadingState;
    private AbstractGameState _currentState;
    [field: SerializeField] public ConfigurationGameState configurationState { get; private set; }
    [field: SerializeField] public FightGameState fightState { get; private set; }
    [field: SerializeField] public FightSummaryGameState fightSummaryState { get; private set; }
    [field: SerializeField] public DirectoryGameState directoryState { get; private set; }

    private void Start()
    {
        ChangeState(configurationState, false).Forget();
    }

    public async UniTask ChangeState(AbstractGameState state, bool playLoadingAnimation = true)
    {
        if (playLoadingAnimation)
        {
            await loadingState.PlayAnimationAsync(new AsyncAnimation.CustomAnimation
                {clipName = "Animation_StartLoading"});
            loadingState.PlayAnimationSync(new AsyncAnimation.CustomAnimation {clipName = "Animation_LoopLoading"});
        }

        if (_currentState != null)
        {
            await _currentState.ExitState();
            _currentState.DeactivateStateObject();
        }

        _currentState = state;
        _currentState.ActivateStateObject();

        if (playLoadingAnimation)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f));
            await loadingState.PlayAnimationAsync(
                new AsyncAnimation.CustomAnimation {clipName = "Animation_EndLoading"});
        }

        await EventBus<GameStateEventBinding>.RaiseAsync(new GameStateEventBinding {GameState = state.stateType});
        EventBus<GameStateEventBinding>.Raise(new GameStateEventBinding {GameState = state.stateType});

        await _currentState.EnterState(this);
    }
}