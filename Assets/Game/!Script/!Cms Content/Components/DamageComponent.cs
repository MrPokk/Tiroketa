using Engine_Component.Utility.Interfaces;
using System;
using System.Data;

namespace Game._Script.CMSGame.Components
{
    public class DamageComponent : EntityComponent, IInitializableToArg<DamageComponent.DamageProperty>
    {
        public DamageProperty Properties { get; private set; }
        public class DamageProperty
        {
            public int ApplyDamage { get; private set; }
            public DamageProperty(int applyDamage)
            {
                ApplyDamage = applyDamage;
            }
        }
        public void Init(DamageProperty property)
        {
            Properties ??= property;
        }

        public DamageProperty ValidateProperty(DamageProperty property)
        {
            if (property.ApplyDamage < 0)
                throw new EvaluateException("ERROR: ApplyDamage not correct set");

            return property;
        }
    }
}
