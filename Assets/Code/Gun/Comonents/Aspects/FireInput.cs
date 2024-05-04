using Unity.Entities;

namespace Code.Gun
{
    public readonly partial struct FireInputAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<FireInput> _fire;

        public float Fire
        {
            get => _fire.ValueRO.Value;
            set => _fire.ValueRW.Value = value;
        }
    }
}