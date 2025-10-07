using Cysharp.Threading.Tasks;

public interface IAsyncAnimator<T>
{
    public UniTask PlayAnimationAsync(T asyncAnimation, bool waitTillAnimationEnds = true);
    public float GetAnimationLength(T asyncAnimation);
}