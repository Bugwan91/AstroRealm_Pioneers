using Code.Damage;
using Unity.Entities;

namespace Code.Player
{
    [UpdateInGroup(typeof(SyncSystemGroup), OrderFirst = true)]
    [UpdateAfter(typeof(DestructSystem))]
    public partial class PlayerInputSystemGroup: ComponentSystemGroup 
    {}
}