using System.Text;
using UnityEngine;

public class FightSummaryScore : AbstractFightSummaryElement
{
    [SerializeField] private ValueBank fightSummaryBank;

    public override int AddElement(GameContext gameContext, StringBuilder stringBuilder, FightSummaryData summaryData)
    {
        stringBuilder.Append($"{summaryData.summaryName}....................{fightSummaryBank.GetBankValue()}");
        return 0;
    }
}