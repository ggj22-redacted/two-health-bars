namespace Game.Common.Projectiles
{
    public interface IHittable
    {
        public void OnHit(ProjectileState state);
    }
}