using Unity.Burst;
using Unity.Entities;

namespace Code.Gun
{
    public partial struct UpdateVelocityOfWeaponSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
        }
    }
    
    [BurstCompile]
    public partial struct UpdateVelocityOfWeaponJob : IJobEntity
    {
        [BurstCompile]
        private void Execute()
        {
            
        }
    }
}