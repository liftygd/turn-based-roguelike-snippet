using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Fight Summary", fileName = "FightSummaryData_")]
public class FightSummaryData : ScriptableObject
{
    public string summaryName;
    [SerializeReference] private AbstractFightSummaryElement summaryElement;

    public int AddElement(GameContext gameContext, StringBuilder stringBuilder)
    {
        return summaryElement.AddElement(gameContext, stringBuilder, this);
    }
}