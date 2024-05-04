using Unity.Entities;

namespace Code.PhysicObject
{
    public struct ResetPosition : IComponentData {}

    public struct ResetRotation : IComponentData {}

    public struct ResetLinearVelocity : IComponentData {}

    public struct ResetAngularVelocity : IComponentData {}
}