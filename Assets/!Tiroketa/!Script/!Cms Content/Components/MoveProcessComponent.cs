using BitterCMS.Utility.Interfaces;
using System;

namespace _Tiroketa._Script._Cms_Content.Components
{
    public class MoveProcessComponent : IEntityComponent, IInitializable<MoveProcessComponent.MoveProcessComponentProperty>
    {

        public class MoveProcessComponentProperty : InitializableProperty
        {
            public readonly Action<float> MoveProcess;
            public MoveProcessComponentProperty(Action<float> moveProcess)
            {
                MoveProcess = moveProcess;
            }
        }
        public MoveProcessComponentProperty Properties { get; set; }
        public void Init(MoveProcessComponentProperty property)
        {
            Properties ??= property;
        }
    }
}
