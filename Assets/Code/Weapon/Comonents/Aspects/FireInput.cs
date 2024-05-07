using Unity.Entities;

namespace Code.Weapon
{
    public readonly partial struct FireInputAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<FireInput> _fire;

        public bool Fire
        {
            get => _fire.ValueRO.Value;
            set => _fire.ValueRW.Value = value;
        }
    }
}