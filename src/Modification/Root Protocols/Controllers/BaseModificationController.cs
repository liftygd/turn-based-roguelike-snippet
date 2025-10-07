using System.Collections.Generic;

public class BaseModificationController<T> where T : IModifier
{
    protected List<T> _modifiers = new();

    public void AddModifier(T modifier)
    {
        _modifiers.Add(modifier);
    }

    public void RemoveModifier(T modifier)
    {
        if (!_modifiers.Contains(modifier)) return;

        _modifiers.Remove(modifier);
    }
}