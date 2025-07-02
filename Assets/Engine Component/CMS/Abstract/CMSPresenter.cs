using Engine_Component.UnityIntegration.BaseComponent;
using Engine_Component.Utility.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Engine_Component.CMSSystem
{
    public abstract class CMSPresenter
    {
        private readonly Dictionary<BaseView, CMSEntity> _loadedEntity = new Dictionary<BaseView, CMSEntity>();
        private readonly HashSet<Type> _allowedEntityTypes = new HashSet<Type>();

        protected CMSPresenter(params Type[] allowedTypes)
        {
            foreach (var type in allowedTypes)
            {
                if (!typeof(CMSEntity).IsAssignableFrom(type))
                    throw new ArgumentException($"Type {type.Name} must inherit from CMSEntity");

                _allowedEntityTypes.Add(type);
            }
        }

        public sealed class CMSPresenterProperty
        {
            public readonly CMSPresenter Presenter;
            public CMSPresenterProperty(CMSPresenter presenter)
            {
                Presenter = presenter;
            }
        }

        #region [SpawnEntity]

        public virtual BaseView SpawnEntity(Type type, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            Create(type, out var entity, position, rotation, parent);
            return entity;
        }

        #endregion

        #region [CreateEntity]

        private void Create(Type entity, out BaseView ID, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            if (entity.IsAbstract)
                throw new TypeAccessException($"Type {entity.Name} is Abstract!");

            if (!IsTypeAllowed(entity))
                throw new InvalidOperationException($"Type {entity.Name} is not allowed for this presenter");

            if (typeof(CMSEntity).IsAssignableFrom(entity))
            {
                if (Activator.CreateInstance(entity) is CMSEntity newObject)
                {
                    newObject.Init(new CMSPresenterProperty(this));
                    newObject.GetComponent<ViewComponent>(out var view);
                    if (view != null)
                    {
                        ID = LinkingMonobehaviour(newObject, view, position, rotation, parent);
                        ID.Init(new CMSPresenterProperty(this));
                        return;
                    }
                }
            }
            else
                throw new TypeAccessException($"Type {entity.Name} is not a CMSEntity!");

            ID = null;
        }

        #endregion

        #region [Entity Management]

        private BaseView LinkingMonobehaviour(
            CMSEntity entity, ViewComponent view,
            Vector3 position, Quaternion rotation, Transform parent
        )
        {
            if (view == null || entity == null || !view.Properties.Original)
                return null;

            var newView = Object.Instantiate(view.Properties.Original, position, rotation, parent);
            newView.name = $"{entity.ID.Name} [NEW]";

            view.Properties.Current = newView;

            _loadedEntity.Add(view.Properties.Current, entity);
            return view.Properties.Current;
        }

        public void SetParent(in BaseView ID, Transform parent, bool worldPositionStays = true)
        {
            if (ID != null && ID.transform != null)
            {
                ID.transform.SetParent(parent, worldPositionStays);
            }
        }

        public void SetPosition(in BaseView ID, Vector3 position)
        {
            if (ID != null && ID.transform != null)
                ID.transform.position = position;
        }

        public void SetRotation(in BaseView ID, Quaternion rotation)
        {
            if (ID != null && ID.transform != null)
                ID.transform.rotation = rotation;
        }

        public void SetLocalScale(in BaseView ID, Vector3 scale)
        {
            if (ID != null && ID.transform != null)
                ID.transform.localScale = scale;
        }

        #endregion

        #region [GetEntity]

        public T GetEntityByID<T>(in BaseView ID) where T : CMSEntity
        {
            return GetEntityByID(ID) as T;
        }

        public CMSEntity GetEntityByID(in BaseView ID)
        {
            return ID == null ? null : _loadedEntity.GetValueOrDefault(ID);
        }

        public T GetEntityByType<T>() where T : CMSEntity
        {
            return GetEntityByType(typeof(T)) as T;
        }

        public CMSEntity GetEntityByType(Type type)
        {
            return _loadedEntity.Values.FirstOrDefault(entity => entity.ID == type);
        }

        public IReadOnlyDictionary<BaseView, CMSEntity> GetEntities()
        {
            return _loadedEntity;
        }
        public IReadOnlyCollection<BaseView> GetViewEntities()
        {
            return GetEntities().Keys as IReadOnlyCollection<BaseView>;
        }
        public IReadOnlyCollection<CMSEntity> GetModelEntities()
        {
            return GetEntities().Values as IReadOnlyCollection<CMSEntity>;
        }

        public IReadOnlyCollection<CMSEntity> GetEntitiesToComponents(params Type[] typeComponent)
        {
            var allEntity = GetModelEntities();
            var list = new List<CMSEntity>();
    
            foreach (var entity in allEntity)
            {
                var hasAllComponents = true;
        
                foreach (var component in typeComponent)
                {
                    if (entity.HasComponent(component))
                        continue;
                    
                    hasAllComponents = false;
                    break;
                }
        
                if (hasAllComponents)
                    list.Add(entity);
            }
    
            return list;
        }

        public IReadOnlyCollection<CMSEntity> GetEntitiesToComponent<TComponent>() where TComponent : EntityComponent
        {
            return GetModelEntities().Where(entity => entity.HasComponent<TComponent>()).ToList();
        }

        #endregion

        #region [DestroyEntity]

        public virtual void DestroyEntity(in BaseView ID)
        {
            if (!ID || !ID.gameObject)
                return;

            _loadedEntity.Remove(ID);
            Object.Destroy(ID.gameObject);
        }

        public virtual void DestroyAllEntities()
        {
            foreach (var entity in _loadedEntity.Keys)
            {
                if (entity && entity.gameObject)
                    Object.Destroy(entity.gameObject);
            }
            _loadedEntity.Clear();
        }

        #endregion

        #region [Helper Methods]

        public bool IsTypeAllowed(Type type)
        {
            if (_allowedEntityTypes.Count == 0)
                return typeof(CMSEntity).IsAssignableFrom(type);

            return _allowedEntityTypes.Any(allowedType => allowedType.IsAssignableFrom(type));
        }

        #endregion
    }
}
