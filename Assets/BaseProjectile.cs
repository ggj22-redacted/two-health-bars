using Unity.Engine.PhysicsModule;
public class BaseProjectile
{
    private SphereCollider hitbox;
    private float speed;
    private float sizeModifier;
    private float maxDistance;
    private int timeToLive;
}