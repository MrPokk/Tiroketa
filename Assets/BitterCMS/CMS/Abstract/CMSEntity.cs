using BitterCMS.System.Serialization;
using BitterCMS.UnityIntegration.CMSComponent;
using BitterCMS.Utility.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BitterCMS.CMSSystem
{
    public abstract class CMSEntity : IInitializable<CMSPresenter.CMSPresenterProperty>, IXmlIncludeExtraType, IXmlSerializable
    {
        [XmlIgnore]
        public Type ID => GetType();

        [NonSerialized]
        private readonly Dictionary<Type, IEntityComponent> _validComponents = new Dictionary<Type, IEntityComponent>();
        public int CountComponent => _validComponents.Count;

        [XmlIgnore]
        public CMSPresenter.CMSPresenterProperty Properties { get; set; }

        public void Init(CMSPresenter.CMSPresenterProperty args) => Properties ??= args;

        public bool TryGetView(out BaseView view)
        {
            if (TryGetComponent(out ViewComponent viewComponent) && 
                viewComponent.Properties != null)
            {
                view = viewComponent.Properties.Current;
                return view;
            }

            view = null;
            return false;
        }
        
        public BaseView GetView() => GetComponent<ViewComponent>()?.Properties?.Current;

        public T GetView<T>() where T : BaseView => GetView() as T;

        public IReadOnlyDictionary<Type, IEntityComponent> GetAllComponent() => _validComponents;

        public T GetUnityComponent<T>() where T : UnityEngine.Component
        {
            var view = GetView();
            return view != null ? view.GetComponent<T>() : null;
        }

        public bool TryGetComponent<T>(out T refComponent) where T : class, IEntityComponent
        {
            if (_validComponents.TryGetValue(typeof(T), out var foundComponent))
            {
                refComponent = (T)foundComponent;
                return true;
            }
    
            refComponent = null;
            return false;
        }
        
        public T GetComponent<T>(out T refComponent) where T : class, IEntityComponent
        {
            if (_validComponents.TryGetValue(typeof(T), out var component))
            {
                refComponent = (T)component;
                return refComponent;
            }
            refComponent = null;
            return null;
        }

        public T GetComponent<T>() where T : class, IEntityComponent
        {
            if (_validComponents.TryGetValue(typeof(T), out var component))
                return (T)component;
            
            return null;
        }

        public T GetOrAddComponent<T>() where T : class, IEntityComponent, new()
        {
            return !HasComponent<T>()
                ? AddComponent<T>()
                : GetComponent<T>();
        }

        public T GetOrAddComponent<T>(out T refComponent) where T : class, IEntityComponent, new()
        {
            return !HasComponent<T>()
                ? AddComponent(out refComponent)
                : GetComponent(out refComponent);
        }

        public void RemoveComponent<T>() where T : IEntityComponent
        {
            _validComponents.Remove(typeof(T));
        }

        public void RemoveComponent(Type type)
        {
            _validComponents.Remove(type);
        }

        public bool HasComponent(Type typeComponent)
        {
            return _validComponents.ContainsKey(typeComponent);
        }

        public bool HasComponent<T>() where T : IEntityComponent
        {
            return _validComponents.ContainsKey(typeof(T));
        }

        public T AddComponent<T>(out T component) where T : IEntityComponent, new()
        {
            component = new T();
            _validComponents.TryAdd(typeof(T), component);
            return component;
        }

        public T AddComponent<T>() where T : IEntityComponent, new()
        {
            var component = new T();
            _validComponents.TryAdd(typeof(T), component);
            return component;
        }

        #region [XmlSerializable]

        public XmlSchema GetSchema() => null;
        public void ReadXml(XmlReader reader)
        {
            SerializerCMSEntityUtility.ReadXml(reader, (type, component) => { _validComponents[type] = component; });
        }
        public void WriteXml(XmlWriter writer)
        {
            SerializerCMSEntityUtility.WriteXml(writer, GetSerializeComponents());
        }

        public Type[] GetExtraType()
        {
            return GetSerializeComponents().Select(component => component.GetType()).ToArray();
        }

        public List<IEntityComponent> GetSerializeComponents()
        {
            return new List<IEntityComponent>(_validComponents.Values
                .Where(component => component.GetType().IsDefined(typeof(SerializableAttribute))));
        }

        #endregion
    }
}
