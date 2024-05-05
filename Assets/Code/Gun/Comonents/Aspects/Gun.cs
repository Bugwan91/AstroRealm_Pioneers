using Code.Utils;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Unity.VisualScripting;

namespace Code.Gun
{
    [BurstCompile]
    public readonly partial struct GunAspect: IAspect
    {
        public readonly Entity Entity;
        
        private readonly RefRO<LocalTransform> _transform;
        private readonly RefRW<Gun> _gun;
        private readonly RefRO<LocalToWorld> _worldTransform;
        
        private float BulletSpeed => _gun.ValueRO.BulletSpeed;

        private float3 Direction => _worldTransform.ValueRO.Forward;

        private float BulletLifeTime => _gun.ValueRO.MaxDistance / BulletSpeed;

        [BurstCompile]
        public void Shoot(float3 baseVelocity, double time, EntityCommandBuffer.ParallelWriter ecb, int index)
        {
            var gun = _gun.ValueRO;
            if (gun.LastShootTime + gun.ShootDelay > time) return;
            _gun.ValueRW.LastShootTime = time;
            var bullet = ecb.Instantiate(index, gun.BulletPrefab);
            ecb.AddComponent(index, bullet, new LifeTime
            {
                TimeLeft = BulletLifeTime
            });
            var posW = _worldTransform.ValueRO;
            var pos = LocalTransform.FromPositionRotation(
                MathUtil.RotateY(gun.MuzzlePosition, posW.Rotation.value, posW.Position),
                posW.Rotation);
            pos.Position.y = 0f;
            ecb.SetComponent(index, bullet, pos);
            var v = PhysicsVelocity.Zero;
            v.Linear = Direction * gun.BulletSpeed + baseVelocity;
            ecb.SetComponent(index, bullet, v);
        }
    }
}