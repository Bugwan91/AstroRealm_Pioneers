using Code.Flight;
using Code.Player;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Code.FloatingOrigin;
using Code.Utils;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine.UIElements;

namespace Code.Camera
{
    [BurstCompile]
    [UpdateInGroup(typeof(CameraSystemGroup), OrderFirst = true)]
    public partial struct MoveCamera : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CameraOrbit>();
            state.RequireForUpdate<CockpitPositionTag>();
            state.RequireForUpdate<CameraTargetTag>();
            state.RequireForUpdate<PlayerInputData>();
            state.RequireForUpdate<Camera>();
            state.RequireForUpdate<CameraSettings>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var input = SystemAPI.GetSingleton<PlayerInputData>();
            
            var player = SystemAPI.GetSingletonEntity<CameraTargetTag>();
            var playerRotation = SystemAPI.GetComponent<LocalTransform>(player).Rotation;
            var acceleration = MathUtil.RotateY(
                SystemAPI.GetComponent<Acceleration>(player).Value,
                math.inverse(playerRotation).value,
                float3.zero);
            
            var camera = SystemAPI.GetAspect<CameraAspect>(SystemAPI.GetSingletonEntity<Camera>());
            camera.Update(input.Scroll, input.CursorY, SystemAPI.Time.DeltaTime, acceleration);

            var cockpit = SystemAPI.GetComponent<LocalTransform>(SystemAPI.GetSingletonEntity<CockpitPositionTag>());
            var cameraOrbit = SystemAPI.GetAspect<CameraOrbitAspect>(SystemAPI.GetSingletonEntity<CameraOrbit>());
            var cameraTargetTransform = SystemAPI.GetComponent<LocalTransform>(SystemAPI.GetSingletonEntity<CameraTargetTag>());
            cameraOrbit.UpdateRotation(input.CursorX);
            cameraOrbit.UpdatePosition(camera.InCockpit
                ? math.rotate(cameraTargetTransform.Rotation, cockpit.Position) + cameraTargetTransform.Position
                : cameraTargetTransform.Position);
        }
    }
}