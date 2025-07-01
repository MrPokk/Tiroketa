using Engine_Component.UnityIntegration.BaseComponent;
using Engine_Component.Utility.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine_Component.CMSSystem
{
    public abstract class CMSEntity : IInitializableToArg<CMSPresenter.CMSPresenterProperty>
    {
        private readonly HashSet<EntityComponent> _components = new HashSet<EntityComponent>();
        public Type ID { get => GetType(); }
        public CMSPresenter.CMSPresenterProperty Properties { get; private set; }
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

        public T GetComponent<T>(out T refComponent) where T : EntityComponent
        {
            return refComponent = _components.FirstOrDefault(c => c is T) as T;
        }

        public void RemoveComponent<T>() where T : EntityComponent
        {
            GetComponent<T>(out T refComponent);
            _components.Remove(refComponent);
        }
        
        public bool HasComponent(Type typeComponent)
        {
            return _components.Any(c => c.ID == typeComponent);
        }
        public bool HasComponent<T>() where T : EntityComponent
        {
            return _components.Any(c => c is T);
        }

        public T GetOrAddComponent<T>(out T refComponent) where T : EntityComponent, new()
        {
            if (!HasComponent<T>())
            {
                AddComponent<T>(out T component);
                return refComponent = component;
            }
            GetComponent<T>(out T existingComponent);
            return refComponent = existingComponent;
        }

        public T AddComponent<T>(out T component) where T : EntityComponent, new()
        {
            component = new T();
            _components.Add(component);
            return component;
        }
    }
}
