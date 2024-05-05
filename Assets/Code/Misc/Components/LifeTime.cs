using Unity.Entities;

namespace Code
{
    public struct LifeTime : IComponentData, IEnableableComponent
    {
        public float TimeLeft;
    }
}