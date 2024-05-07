using Unity.Entities;

namespace Code.Weapon
{
    public struct FireInput: IComponentData //, IEnableableComponent ??
    {
        public bool Value;
    }
}