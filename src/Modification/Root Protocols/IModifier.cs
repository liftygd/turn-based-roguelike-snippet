public interface IModifier
{
    ModifierOrder Order { get; }
    bool IsStackable { get; }
}

public enum ModifierOrder
{
    Lowest = 0,
    Low = 1,
    Middle = 2,
    High = 3,
    Highest = 4
}