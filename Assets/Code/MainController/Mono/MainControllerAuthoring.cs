using Code.Player;
using Unity.Entities;
using UnityEngine;

namespace Code.MainController
{
    public class MainControllerAuthoring : MonoBehaviour
    {}
    
    public class MainControllerBaker : Baker<MainControllerAuthoring>
    {
        public override void Bake(MainControllerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
        }
    }
}