using Unity.Entities;
using UnityEngine;

namespace Code.Damage
{
    public class DamageDealerAuthoring : MonoBehaviour
    {
        public float damage;
        private class DamageDealerAuthoringBaker : Baker<DamageDealerAuthoring>
        {
            public override void Bake(DamageDealerAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new DamageDealer
                {
                    Value = authoring.damage
                });
            }
        }
    }
}