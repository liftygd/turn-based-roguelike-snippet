public interface IFactory<TInput, TResult>
{
    public TResult Create(TInput data);
}