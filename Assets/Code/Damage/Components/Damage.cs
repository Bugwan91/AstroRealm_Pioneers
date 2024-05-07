using Unity.Entities;

namespace Code.Damage

{
    public struct DamageDealer : IComponentData
    {
        public float Value;
    }

    // TODO: Update to IBufferElementData
    // TODO: Maybe it worth to use IEnableableComponent for performance reasons
    public struct TakingDamage : IComponentData
    {
        public float Value;
    }
    
    public struct Destructable : IComponentData, IEnableableComponent
    {
        public float Health;
        public bool ByTimer;
        public float TimeLeft;
    }
    
}