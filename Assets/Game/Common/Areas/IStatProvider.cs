namespace Game.Common.Areas
{
    public interface IStatProvider
    {
        public Stat GetStat(EntityState entityState);
    }
}