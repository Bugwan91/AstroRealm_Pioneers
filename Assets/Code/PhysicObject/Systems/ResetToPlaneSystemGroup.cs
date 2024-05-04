using Unity.Entities;
using Unity.Physics.Systems;

namespace Code.PhysicObject
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial class ResetToPlaneSystemGroup: ComponentSystemGroup {}
}