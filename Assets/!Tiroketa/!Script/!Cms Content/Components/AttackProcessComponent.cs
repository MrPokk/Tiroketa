using BitterCMS.Utility.Interfaces;
using System;

namespace Game._Script.CMSGame.Components
{
    public class AttackProcessComponent : IEntityComponent, IInitializable<AttackProcessComponent.AttackProcessComponentProperty>
    {
        public class AttackProcessComponentProperty : InitializableProperty
        {
            public readonly Action AttackProcess;
            public AttackProcessComponentProperty(Action attackProcess)
            {
                AttackProcess = attackProcess;
            }
        }
        public AttackProcessComponentProperty Properties { get; set; }
        public void Init(AttackProcessComponentProperty property)
        {
            Properties ??= property;
        }
    }

}
