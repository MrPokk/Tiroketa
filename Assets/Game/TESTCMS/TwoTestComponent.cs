using Engine_Component.Utility.Interfaces;
using System;

namespace Game.TESTCMS
{
    [Serializable]
    public class TwoTestComponent : EntityComponent, IInitializableToArg<TwoTestComponent.PropertyTest>
    {
        public float Test = 3;

        
        [Serializable]
        public class PropertyTest : TagProperty
        {
            public float Test = 3;
        }
        
        public PropertyTest Properties { get; set; }
        public void Init(PropertyTest property)
        {
            Properties = property;
        }
    }
    [Serializable]
    public class TagProperty
    { }

}
