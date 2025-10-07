using System;
using System.Text;

[Serializable]
public abstract class AbstractFightSummaryElement
{
    /// <summary>
    ///     Adds a summary element to text using game context and string builder, returns the amount of score to add.
    /// </summary>
    public abstract int AddElement(GameContext gameContext, StringBuilder stringBuilder, FightSummaryData summaryData);
}