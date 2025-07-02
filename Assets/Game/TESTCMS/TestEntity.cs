using Engine_Component.CMSSystem;
using System;


namespace Game.TESTCMS
{
    [Serializable]
    public class TestEntity : CMSEntity
    {
        public TestEntity()
        {
            AddComponent(out OneTestComponent _);
            AddComponent(out TwoTestComponent _);
        }
    }
}
