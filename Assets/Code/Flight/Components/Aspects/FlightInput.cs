using Unity.Entities;
using Unity.Mathematics;

namespace Code.Flight
{
    public readonly partial struct FlightInputAspect : IAspect
    {
        public readonly Entity Entity;
        
        private readonly RefRW<StrafeInput> _strafe;
        private readonly RefRW<DirectionInput> _turn;
        private readonly RefRW<InertialDamperInput> _inertialDamper;

        public float3 Strafe
        {
            get => _strafe.ValueRO.Value;
            set => _strafe.ValueRW.Value = value;
        }

        public float3 Direction
        {
            get => _turn.ValueRO.Value;
            set => _turn.ValueRW.Value = value;
        }

        public float InertialDumper
        {
            get => _inertialDamper.ValueRO.Value;
            set => _inertialDamper.ValueRW.Value = value;
        }
    }
}