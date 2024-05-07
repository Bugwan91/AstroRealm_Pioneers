using Unity.Entities;
using Unity.Physics;

namespace Code.PhysicObject
{
    public struct DisabledCollider : IComponentData, IEnableableComponent
    {
        public PhysicsCollider Value;
    }
}