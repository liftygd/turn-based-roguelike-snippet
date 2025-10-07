public class EntityStatsController
{
    private readonly Stats _stats;

    public EntityStatsController(BaseStats statValues)
    {
        _stats = new Stats(new StatsMediator(), statValues);
    }

    public int GetStat(StatType statType)
    {
        return _stats.GetValue(statType);
    }

    public StatsMediator GetMediator()
    {
        return _stats.Mediator;
    }
}