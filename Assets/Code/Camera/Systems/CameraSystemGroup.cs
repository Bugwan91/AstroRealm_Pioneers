using Unity.Entities;
using Unity.Physics.Systems;

namespace Code.Camera
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial class CameraSystemGroup: ComponentSystemGroup {}
}