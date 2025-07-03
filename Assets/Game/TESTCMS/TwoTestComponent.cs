using Engine_Component.Utility.Interfaces;
using System;
using System.Xml.Serialization;

namespace Game.TESTCMS
{
    [Serializable]
    public class TwoTestComponent : IEntityComponent
    {
        public int Test;
    }
}
