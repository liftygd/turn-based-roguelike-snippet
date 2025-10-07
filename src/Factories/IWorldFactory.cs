using UnityEngine;

public interface IWorldFactory<TInput, TResult>
{
    public TResult Create(TInput data, Vector2Int position, bool shouldPlace);
}