using Unity.Entities;
using Unity.Physics.Systems;

namespace Code.Camera
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial class CameraSystemGroup: ComponentSystemGroup {}
}