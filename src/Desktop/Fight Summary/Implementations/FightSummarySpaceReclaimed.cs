using System.Text;

public class FightSummarySpaceReclaimed : AbstractFightSummaryElement
{
    public float scorePerSpaceReclaimed = 1.1f;

    public override int AddElement(GameContext gameContext, StringBuilder stringBuilder, FightSummaryData summaryData)
    {
        var reclaimedSpace = gameContext.SpaceManager.GetSpaceReclaimed();
        var formatNumber = NumberToSizeFormatter.FormatNumber(reclaimedSpace);

        stringBuilder.Append(
            $"{summaryData.summaryName}....................{formatNumber.number} {formatNumber.sizeUnit}");
        return (int) (reclaimedSpace * scorePerSpaceReclaimed);
    }
}