using Engine_Component.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Engine_Component.CMSSystem
{
    public sealed class CMSView : BaseInteraction, IInitInMain
    {
        public override Priority PriorityInteraction { get => Priority.High; }

        private static HashSet<BaseView> _cmsView;

        public void Init()
        {
            _cmsView = new HashSet<BaseView>();
            FindAll();
        }
        
        public static BaseView Get(Type viewType)
        {
            var viewResult = _cmsView.FirstOrDefault(view => view.ID == viewType);
            if (viewResult == null)
                throw new ArgumentNullException($"ERROR: View is null CHECK PATH: Resources/{PathResources.View}");
            return viewResult;
        }
        public static T Get<T>() where T : BaseView
        {
            return Get(typeof(T)) as T;
        }

        public static IReadOnlyCollection<BaseView> GetAll()
        {
            return _cmsView;
        }

        private static void FindAll()
        {
            try
            {
                var all = Resources.LoadAll<GameObject>(PathResources.View);
                if (all == null || all.Length == 0)
                {
                    Debug.LogError($"No CMSView prefabs found at path: Resources/{PathResources.View}");
                    return;
                }

                _cmsView.Clear();

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

                    _cmsView.Add(component);
                    view.SetActive(wasActive);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"CMSView initialization failed: {ex.Message}");
            }
        }
    }
}
