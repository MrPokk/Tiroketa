using BitterCMS.UnityIntegration.CMSComponent;
using BitterCMS.Utility.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BitterCMS.CMSSystem
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
        
        public sealed class CMSPresenterProperty : InitializableProperty
        {
            public readonly CMSPresenter Presenter;
            public CMSPresenterProperty(CMSPresenter presenter)
            {
                Presenter = presenter;
            }
            
        }

        #region [SpawnEntity]
        /// <summary>
        /// Spawn entity from database if the type is [Serializable], other creates of type
        /// </summary>
        public virtual BaseView SpawnFromDB(Type type, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            if (type.IsDefined(typeof(SerializableAttribute)))
            {
                var newEntity = CMSEntityDatabase.GetEntity(type);
                Create(newEntity, out var newEntityView, position, rotation, parent);
                return newEntityView;
            }

            Create(type, out var entity, position, rotation, parent);
            return entity;
        }
        /// <summary>
        /// Spawn a new entity of type
        /// </summary>
        public virtual BaseView SpawnEntity(Type type, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            Create(type, out var entity, position, rotation, parent);
            return entity;
        }
        /// <summary>
        /// Spawn an entity from an existing CMSEntity instance
        /// </summary>
        public virtual BaseView SpawnEntity(CMSEntity valueEntity, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            Create(valueEntity, out var entity, position, rotation, parent);
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

        private void Create(CMSEntity cmsEntity, out BaseView ID, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            cmsEntity.Init(new CMSPresenterProperty(this));
            cmsEntity.GetComponent<ViewComponent>(out var view);
            if (view != null)
            {
                ID = LinkingMonobehaviour(cmsEntity, view, position, rotation, parent);
                ID.Init(new CMSPresenterProperty(this));
                return;
            }

            Debug.LogError($"ERROR: ViewComponent in not {cmsEntity}");
            ID = null;
        }

        #endregion

        #region [Entity Management]

        private BaseView LinkingMonobehaviour(
            CMSEntity entity, ViewComponent view, 
            Vector3 position, Quaternion rotation, Transform parent)
        {
            if (view?.Properties?.Original == null || entity == null)
                return null;

            var newView = Object.Instantiate(view.Properties.Original, position, rotation, parent);
            newView.name = $"{entity.ID.Name} [NEW]";
    
            view.Properties.Current = newView;
            _loadedEntity[newView] = entity;
    
            return newView;
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
        
        public CMSEntity[] GetEntitiesToComponent(
            Type[] requiredComponents = null, 
            Type[] excludedComponents = null)
        {
            var allEntity = GetModelEntities();
    
            return allEntity.Where(entity => 
            {
                var hasRequired = requiredComponents == null || 
                                  requiredComponents.All(entity.HasComponent);
        
                var hasExcluded = excludedComponents != null && 
                                  excludedComponents.Any(entity.HasComponent);
        
                return hasRequired && !hasExcluded;
            }).ToArray();
        }

        public IReadOnlyCollection<CMSEntity> GetEntitiesToComponent<TRequired, TExcluded>() 
            where TRequired : IEntityComponent 
            where TExcluded : IEntityComponent
        {
            return GetModelEntities()
                .Where(entity => entity.HasComponent<TRequired>() && !entity.HasComponent<TExcluded>())
                .ToArray();
        }
        
        public CMSEntity[] GetEntitiesToComponent(params Type[] typeComponent)
        {
            var allEntity = GetModelEntities();

            return (from entity in allEntity let hasAllComponents = 
                typeComponent.All(entity.HasComponent) where hasAllComponents select entity).ToArray();
        }

        public IReadOnlyCollection<CMSEntity> GetEntitiesToComponent<TComponent>() where TComponent : IEntityComponent
        {
            return GetModelEntities().Where(entity => entity.HasComponent<TComponent>()).ToArray();
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
