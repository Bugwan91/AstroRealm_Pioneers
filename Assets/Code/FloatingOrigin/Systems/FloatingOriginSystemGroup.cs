using Code.Flight;
using Unity.Entities;
using Unity.Physics.Systems;

namespace Code.FloatingOrigin
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup), OrderFirst = true)]
    [UpdateBefore(typeof(PhysicsSystemGroup))]
    public partial class FloatingOriginSystemGroup: ComponentSystemGroup {}
}