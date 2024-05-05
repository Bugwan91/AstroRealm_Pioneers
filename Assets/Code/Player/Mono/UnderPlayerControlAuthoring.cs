using Unity.Entities;
using UnityEngine;

namespace Code.Player
{
    public class UnderPlayerControlAuthoring : MonoBehaviour
    {
        private class UnderPlayerControlBaker : Baker<UnderPlayerControlAuthoring>
        {
            public override void Bake(UnderPlayerControlAuthoring controlAuthoring)
            {
                AddComponent<ShipControlledByPlayerTag>(GetEntity(TransformUsageFlags.Dynamic));
            }
        }
    }
}