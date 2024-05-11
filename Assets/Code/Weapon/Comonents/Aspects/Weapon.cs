using Code.Utils;
using Code.Damage;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Code.Weapon
{
    [BurstCompile]
    public readonly partial struct WeaponAspect: IAspect
    {
        public readonly Entity Entity;
        
        private readonly RefRO<LocalTransform> _transform;
        private readonly RefRW<Weapon> _weapon;
        private readonly RefRO<LocalToWorld> _worldTransform;
        
        private float BulletSpeed => _weapon.ValueRO.BulletSpeed;

        private float3 Direction => _worldTransform.ValueRO.Forward;

        private float BulletLifeTime => _weapon.ValueRO.MaxDistance / BulletSpeed;

        [BurstCompile]
        public void Shoot(float3 baseVelocity, double time, EntityCommandBuffer.ParallelWriter ecb, int index)
        {
            var weapon = _weapon.ValueRO;
            if (weapon.LastShootTime + weapon.ShootDelay > time) return;
            _weapon.ValueRW.LastShootTime = time;
            var bullet = ecb.Instantiate(index, weapon.ProjectilePrefab);
            ecb.SetComponent(index, bullet, new Destructable
            {
                ByTimer = true,
                TimeLeft = BulletLifeTime
            });
            var posW = _worldTransform.ValueRO;
            var pos = LocalTransform.FromPositionRotation(
                MathUtil.RotateY(weapon.MuzzlePosition, posW.Rotation.value, posW.Position),
                posW.Rotation);
            pos.Scale = weapon.ProjectileSize;
            // pos.Position.y = 0f;
            ecb.SetComponent(index, bullet, pos);
            var v = PhysicsVelocity.Zero;
            v.Linear = Direction * weapon.BulletSpeed + baseVelocity;
            ecb.SetComponent(index, bullet, v);
            ecb.SetComponent(index, bullet, new Damage.DamageDealer
            {
                Value = _weapon.ValueRO.Damage
            });
        }
    }
}