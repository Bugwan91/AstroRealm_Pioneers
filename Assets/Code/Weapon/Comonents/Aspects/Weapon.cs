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
        private readonly RefRO<LocalToWorld> _world;

        public Entity ProjectilePrefab => _weapon.ValueRO.ProjectilePrefab;

        public double LastShootTime
        {
            get => _weapon.ValueRO.LastShootTime;
            set => _weapon.ValueRW.LastShootTime = value;
        }
        
        public float BulletSpeed => _weapon.ValueRO.BulletSpeed;
        
        public float3 Direction => _world.ValueRO.Forward;
        
        public float BulletLifeTime => _weapon.ValueRO.MaxDistance / BulletSpeed;
        
        public float Damage => _weapon.ValueRO.Damage;
        
        public bool CanShoot(double time)
        {
            return _weapon.ValueRO.LastShootTime + _weapon.ValueRO.ShootDelay < time;
        }
        
        public LocalTransform ProjectilePosition
        {
            get
            {
                var t = LocalTransform.FromPositionRotation(
                    MathUtil.RotateY(
                        _weapon.ValueRO.MuzzlePosition,
                        _world.ValueRO.Rotation.value,
                        _world.ValueRO.Position),
                    _world.ValueRO.Rotation);
                t.Scale = _weapon.ValueRO.ProjectileSize;
                return t;
            }
        }
    }
}