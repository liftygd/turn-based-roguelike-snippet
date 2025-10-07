using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AsyncAnimator : MonoBehaviour, IAsyncAnimator<AsyncAnimation>
{
    [SerializeField] private Animator animator;
    private readonly CancellationTokenSource _cts = new();

    private void OnDestroy()
    {
        _cts.Cancel();
    }

    public async UniTask PlayAnimationAsync(AsyncAnimation asyncAnimation, bool waitTillAnimationEnds = false)
    {
        _cts.Token.ThrowIfCancellationRequested();

        var controller = animator.runtimeAnimatorController as AnimatorOverrideController;
        if (!controller) return;

        var clip = controller[asyncAnimation.clipName];
        if (!clip) return;

        animator.Play(asyncAnimation.clipName, -1, 0f);

        await UniTask
            .NextFrame(); //Needed to correctly think clip time to animation, because animation doesn't start playing until next frame
        await UniTask.Delay(TimeSpan.FromSeconds(clip.length), cancellationToken: _cts.Token);
    }

    public float GetAnimationLength(AsyncAnimation asyncAnimation)
    {
        var controller = animator.runtimeAnimatorController as AnimatorOverrideController;
        if (!controller) return -1;

        var clip = controller[asyncAnimation.clipName];
        if (!clip) return -1;

        return clip.length;
    }
}