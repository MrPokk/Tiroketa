using Engine_Component.Utility.Interfaces;
using System;
using System.Xml.Serialization;

namespace Game.TESTCMS
{
    [Serializable]
    [XmlInclude(typeof(TwoTestComponent))]
    public class TwoTestComponent : EntityComponent
    {
        public int Test;
    }

}
