using Unity.Entities;
using Unity.Mathematics;

namespace Code.Player
{
    public struct PlayerInputData : IComponentData
    {
        public float2 Strafe;
        public float InertialDamper;
        public float CursorX;
        public float CursorY;
        public float Scroll;
        public float FreeLook;
        public float Fire;

        // TODO: Toggle inertial dumper
    }
}