using Unity.Entities;
using UnityEngine;

namespace Code.Damage
{
    public class DestructableAuthoring : MonoBehaviour
    {
        public float health = 1f;
        public float timeLeft = 0f;
        
        private class DestructableAuthoringBaker : Baker<DestructableAuthoring>
        {
            public override void Bake(DestructableAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Destructable
                {
                    Health = authoring.health,
                    ByTimer = authoring.timeLeft > 0,
                    TimeLeft = authoring.timeLeft,
                });
                AddComponent<TakingDamage>(entity);
            }
        }
    }
}