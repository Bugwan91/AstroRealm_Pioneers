using Unity.Burst;
using Unity.Entities;
using Unity.Physics.Systems;

namespace Code.Damage
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial class DamageSystemGroup: ComponentSystemGroup {}
}