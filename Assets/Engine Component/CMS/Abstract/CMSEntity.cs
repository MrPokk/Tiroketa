using Engine_Component.UnityIntegration.BaseComponent;
using Engine_Component.Utility.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Engine_Component.CMSSystem
{
    public abstract class CMSEntity : IInitializableToArg<CMSPresenter.CMSPresenterProperty>
    {
        [XmlIgnore]
        public Type ID => GetType();

        private readonly Dictionary<Type, EntityComponent> _validComponents = new Dictionary<Type, EntityComponent>();

        [XmlArray("Components")]
        [XmlArrayItem("Component", Type = typeof(EntityComponent))]
        public List<EntityComponent> AllSerializeComponents => GetSerializeComponents();

        [XmlIgnore]
        public CMSPresenter.CMSPresenterProperty Properties { get; set; }

        public void Init(CMSPresenter.CMSPresenterProperty args)
        {
            Properties ??= args;
        }

        public BaseView GetView()
        {
            return GetComponent(out ViewComponent _)?.Properties?.Current;
        }

        public T GetView<T>() where T : BaseView
        {
            return GetView() as T;
        }

        public T GetUnityComponent<T>() where T : UnityEngine.Component
        {
            var view = GetView();
            return view != null ? view.GetComponent<T>() : null;
        }

        public IReadOnlyDictionary<Type, EntityComponent> GetAllComponent()
        {
            return _validComponents;
        }

        public T GetComponent<T>(out T refComponent) where T : EntityComponent
        {
            if (_validComponents.TryGetValue(typeof(T), out var component))
            {
                refComponent = (T)component;
                return refComponent;
            }
            refComponent = null;
            return null;
        }

        public void RemoveComponent<T>() where T : EntityComponent
        {
            _validComponents.Remove(typeof(T));
        }

        public bool HasComponent(Type typeComponent)
        {
            return _validComponents.ContainsKey(typeComponent);
        }

        public bool HasComponent<T>() where T : EntityComponent
        {
            return _validComponents.ContainsKey(typeof(T));
        }

        public T GetOrAddComponent<T>(out T refComponent) where T : EntityComponent, new()
        {
            return !HasComponent<T>()
                ? AddComponent(out refComponent)
                : GetComponent(out refComponent);
        }

        public T AddComponent<T>(out T component) where T : EntityComponent, new()
        {
            component = new T();
            _validComponents.TryAdd(typeof(T), component);
            return component;
        }

        private List<EntityComponent> GetSerializeComponents()
        {
            return new List<EntityComponent>(_validComponents.Values
                .Where(component => component.GetType().IsDefined(typeof(SerializableAttribute))));
        }
    }
}
