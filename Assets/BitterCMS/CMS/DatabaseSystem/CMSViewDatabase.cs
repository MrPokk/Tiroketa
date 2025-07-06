using System;
using System.Collections.Generic;
using UnityEngine;

namespace BitterCMS.CMSSystem
{
    public class CMSViewDatabase : CMSDatabase
    {
        private readonly static Dictionary<Type, BaseView> AllView = new Dictionary<Type, BaseView>();
        
        private static bool _isInit;
        private static void CheckInit()
        {
            if (!_isInit)
                new CMSViewDatabase().Initialize();
        }

        public static BaseView Get(Type viewType)
        {
            CheckInit();

            if (!AllView.TryGetValue(viewType, out var viewResult))
                throw new KeyNotFoundException($"View of type {viewType.Name} not found in database");

            if (!viewResult)
                throw new ArgumentNullException($"ERROR: View is null CHECK PATH: Resources/{PathInProject.CMS_VIEWS}");

            return viewResult;
        }

        public static T Get<T>() where T : BaseView
        {
            CheckInit();

            return Get(typeof(T)) as T;
        }

        public static ICollection<BaseView> GetAll()
        {
            CheckInit();
            
            return AllView.Values;
        }

        public override void Initialize(bool forceUpdate = false)
        {
            if (_isInit && !forceUpdate)
                return;
            
            try
            {
                AllView.Clear();
                
                var allGameObjects = Resources.LoadAll<GameObject>(PathInProject.CMS_VIEWS);
                if (allGameObjects == null || allGameObjects.Length == 0)
                {
                    Debug.LogError($"CMSViewDatabase not found found at path: {PathInProject.CMS_VIEWS}");
                    AllView.Clear();
                }

                if (allGameObjects != null)
                    foreach (var view in allGameObjects)
                    {
                        if (!view)
                            continue;
                        
                        var component = view.GetComponent<BaseView>();
                        if (!component)
                            throw new MissingComponentException($"BaseView missing in prefab: {view.name}");

                        AllView.TryAdd(component.ID, component);
                    }
                
                _isInit = true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database initialization failed: {ex.Message}", ex);
            }
        }
    }
}
