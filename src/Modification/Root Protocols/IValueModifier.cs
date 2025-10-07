public interface IValueModifier : IModifier
{
    public void ApplyModifier(GridEntity entity, object caller, StatsMediator mediator);
}