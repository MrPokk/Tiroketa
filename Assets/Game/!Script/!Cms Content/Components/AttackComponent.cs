using Engine_Component.Utility.Interfaces;
using System;

namespace Game._Script.CMSGame.Components
{
    public class AttackComponent : IEntityComponent, IInitializableToArg<AttackComponent.AttackProperty>
    {
        public AttackProperty Properties { get; set; }

        public class AttackProperty
        {
            public Action AttackProcess { get; private set; }

            public AttackProperty(Action attackProcess)
            {
                AttackProcess = attackProcess;
            }
        }
        public void Init(AttackProperty property)
        {
            Properties ??= property;
        }
    }
}
