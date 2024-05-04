using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Code.Player
{
    [BurstCompile]
    [UpdateInGroup(typeof(PlayerInputSystemGroup), OrderFirst = true)]
    public partial class ReadPlayerInputSystem : SystemBase
    {
        private PlayerControls _controls;

        protected override void OnCreate()
        {
            _controls = new PlayerControls();
        }

        protected override void OnStartRunning()
        {
            _controls.Enable();
        }

        protected override void OnUpdate()
        {
            SystemAPI.SetSingleton(new PlayerInputData
            {
                Strafe = new float2(_controls.ShipControls.StrafeX.ReadValue<float>(), _controls.ShipControls.StrafeZ.ReadValue<float>()),
                InertialDamper = _controls.ShipControls.InertialDamper.ReadValue<float>(),
                
                Fire = _controls.ShipControls.Fire.ReadValue<float>(),
                    
                CursorX = _controls.CameraControls.XAxis.ReadValue<float>(),
                CursorY = _controls.CameraControls.YAxis.ReadValue<float>(),
                Scroll = _controls.CameraControls.Scroll.ReadValue<float>(),
                FreeLook = _controls.CameraControls.FreeLook.ReadValue<float>()
            });
        }

        protected override void OnStopRunning()
        {
            _controls.Disable();
        }
    }
}