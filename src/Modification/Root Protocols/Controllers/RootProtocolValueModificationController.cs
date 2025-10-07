public class RootProtocolValueModificationController : BaseModificationController<IValueModifier>
{
    public void ApplyModifiers(GridEntity entity, object caller, StatsMediator mediator)
    {
        foreach (var modifier in _modifiers)
            modifier.ApplyModifier(entity, caller, mediator);
    }
}