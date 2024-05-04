using Unity.Entities;
using Unity.Physics.Systems;

namespace Code.Flight
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(PhysicsSystemGroup))]
    public partial class FlightSystemGroup: ComponentSystemGroup {}
}