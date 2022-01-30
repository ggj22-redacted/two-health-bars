namespace Game.Common.Areas
{
    public interface IStatMutator
    {
        public float Mutate(EntityState entityState, Stat stat, float value, float min, float max, float rate);
    }
}