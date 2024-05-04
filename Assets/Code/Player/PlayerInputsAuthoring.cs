using Unity.Entities;
using UnityEngine;

namespace Code.Player
{
    public class PlayerInputsAuthoring : MonoBehaviour
    {}

    public class BakePlayer : Baker<PlayerInputsAuthoring>
    {
        public override void Bake(PlayerInputsAuthoring inputsAuthoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<PlayerInputData>(entity);
        }
    }
}