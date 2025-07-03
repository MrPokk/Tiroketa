using System;

namespace Engine_Component.Utility.Interfaces
{
    public interface IEntityComponent
    {
        public Type ID => GetType();
    }
    
    public interface ISerializerProvider
    {
        Type GetObjectType();
        string GetFullPath();

        string Serialization();
        object Deserialize();
    }
    
    public interface IRoot
    {
        protected internal void PreStartGame();
        protected internal void UpdateGame( float timeDelta);
        protected internal void PhysicUpdateGame(float timeDelta);
        protected internal void StoppedGame();
    }

    public interface IXmlIncludeExtraType
    {
        public Type[] GetExtraType();
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
