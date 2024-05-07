using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Weapon
{
    public class WeaponSlotAuthoring : MonoBehaviour
    {
        public GameObject weapon;
    }

    public class WeaponSlotBaker : Baker<WeaponSlotAuthoring>
    {
        public override void Bake(WeaponSlotAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<WeaponSlot>(entity);
        }
    }
}