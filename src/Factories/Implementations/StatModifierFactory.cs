using System;

public class StatModifierFactory : IFactory<StatQuery, StatModifier>
{
    public StatModifier Create(StatQuery data)
    {
        IOperationStrategy strategy = data.OperatorType switch
        {
            OperatorType.Add => new AddOperation(data.Value),
            OperatorType.Multiply => new MultiplyOperation(data.Value),
            _ => throw new ArgumentOutOfRangeException()
        };

        return new StatModifier(data.StatType, strategy, data.Duration);
    }
}