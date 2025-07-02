using System;
using System.Collections.Generic;
using UnityEngine;

namespace Engine_Component.CMSSystem
{
    public static class CMSViewDatabase
    {
        private readonly static Dictionary<Type, BaseView> AllView = new Dictionary<Type, BaseView>();

        public static BaseView Get(Type viewType)
        {
            if (AllView.TryGetValue(viewType, out var viewResult))
            {
                if (viewResult == null)
                    throw new ArgumentNullException($"ERROR: View is null CHECK PATH: Resources/{PathInProject.CMS_VIEWS}");

                return viewResult;
            }
            throw new KeyNotFoundException($"View of type {viewType.Name} not found in database");
        }

        public static T Get<T>() where T : BaseView
        {
            return Get(typeof(T)) as T;
        }

        public static IReadOnlyCollection<BaseView> GetAll()
        {
            return AllView.Values;
        }

        public static void FindAll(bool forceUpdate = false)
        {
            try
            {
                var all = Resources.LoadAll<GameObject>(PathInProject.CMS_VIEWS);
                if (all == null || all.Length == 0)
                {
                    Debug.LogError($"No CMSViewDatabase prefabs found at path: {PathInProject.CMS_VIEWS}");
                    return;
                }

                var newViews = new Dictionary<Type, BaseView>();

                foreach (var view in all)
                {
                    if (!view) continue;

                    var wasActive = view.activeSelf;
                    view.SetActive(true);

                    var component = view.GetComponent<BaseView>();
                    if (!component)
                    {
                        view.SetActive(wasActive);
                        throw new MissingComponentException($"BaseView missing in prefab: {view.name}");
                    }

                    newViews.TryAdd(component.GetType(), component);
                    view.SetActive(wasActive);
                }

                if (newViews.Count <= 0 && !forceUpdate)
                    return;

                AllView.Clear();
                foreach (var item in newViews)
                {
                    AllView.Add(item.Key, item.Value);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"CMSViewDatabase initialization failed: {ex.Message}");
            }
        }
    }
}
