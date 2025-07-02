using Engine_Component.Utility.Interfaces;
using System;
using System.Data;


namespace Game._Script.CMSGame.Components
{
    public class MoveComponent : EntityComponent, IInitializableToArg<MoveComponent.MoveProperty>
    {

        public MoveProperty Properties { get; set; }

        public class MoveProperty
        {
            public Action MoveProcess { get; private set; }
            public int Speed { get; private set; }
            public MoveProperty(Action moveProcess, int speed)
            {
                MoveProcess = moveProcess;
                Speed = speed;
            }
        }
        public void Init(MoveProperty property)
        {
            Properties ??= ValidateProperty(property);
        }

        public MoveProperty ValidateProperty(MoveProperty property)
        {
            if (property.Speed < 0)
                throw new EvaluateException("ERROR: Speed not correct set ");

            return property;
        }
    }
}
