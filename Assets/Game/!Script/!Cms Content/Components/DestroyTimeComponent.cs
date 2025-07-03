using Engine_Component.Utility.Interfaces;
using System;

namespace Game._Script._Cms_Content.Components
{
    [Serializable]
    public class DestroyTimeComponent : IEntityComponent
    {
        public float TotalTime;
        
        public void Append(float amount)
        {
            if (amount < 0)
                return;

            TotalTime += amount;
        }

        public void Decrease(float amount)
        {
            TotalTime = Math.Max(0, TotalTime - amount);
        }
    }

}
