using Engine_Component.Utility.Interfaces;
using System;
using UnityEngine;

namespace Game._Script._Cms_Content.Components
{
    public class DestroyTimeComponent : EntityComponent, IInitializableToArg<DestroyTimeComponent.DestroyTimeProperty>
    {
        public class DestroyTimeProperty
        {
            public float TotalTime { get; private set; } 
            public DestroyTimeProperty(int timeDestroy)
            {
                TotalTime = timeDestroy;
            }

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
        public DestroyTimeProperty Properties { get; private set; }
        public void Init(DestroyTimeProperty property)
        {
            Properties ??= property;
        }
    }

}
