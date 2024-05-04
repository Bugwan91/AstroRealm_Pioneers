using Unity.Entities;

namespace Code.Player
{
    [UpdateInGroup(typeof(SyncSystemGroup), OrderFirst = true)]
    public partial class PlayerInputSystemGroup: ComponentSystemGroup 
    {}
}