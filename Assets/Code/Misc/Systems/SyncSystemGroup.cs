using Unity.Entities;

namespace Code
{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    [UpdateBefore(typeof(EndInitializationEntityCommandBufferSystem))]
    public partial class SyncSystemGroup: ComponentSystemGroup { }
}