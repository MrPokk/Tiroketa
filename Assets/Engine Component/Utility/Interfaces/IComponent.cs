using System;

namespace Engine_Component.Utility.Interfaces
{
    public abstract class EntityComponent
    {
        public Type ID => GetType();
    }

    public interface IInitializable
    {
        public void Init();
    }
    public interface IInitializableToArg<T> where T : class
    {
        public T Properties { get; set; }
        public void Init(T property);
        public T ValidateProperty(T property) { return property; }
    }
}
