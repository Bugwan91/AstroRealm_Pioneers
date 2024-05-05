using Code.Utils;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Code.Camera
{
    [BurstCompile]
    [UpdateInGroup(typeof(CameraSystemGroup))]
    public partial struct MovePointer : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Camera>();
            state.RequireForUpdate<CameraOrbit>();
            state.RequireForUpdate<PointerTag>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var camera = SystemAPI.GetAspect<CameraAspect>(SystemAPI.GetSingletonEntity<Camera>());
            var orbit = SystemAPI.GetComponent<LocalTransform>(SystemAPI.GetSingletonEntity<CameraOrbit>());
            var pointer = SystemAPI.GetAspect<PointerAspect>(SystemAPI.GetSingletonEntity<PointerTag>());
            
            var pointerPivot = LocalTransform.FromPositionRotation(
                MathUtil.RotateY(camera.OriginPosition.xyz, orbit.Rotation.value, orbit.Position),
                MathUtil.Rotate(orbit.Rotation, camera.PointerRotation));
            pointer.Position = MathUtil.RayTraceToMainPlane(pointerPivot.Position,  pointerPivot.Forward());
        }
    }
}