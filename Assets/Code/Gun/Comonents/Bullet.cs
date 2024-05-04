using Unity.Entities;

namespace Code.Gun
{
    public struct Bullet: IComponentData
    {
        public float TimeLeft;
    }
}