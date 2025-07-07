using BitterCMS.Utility;
using System;
using System.Collections.Generic;

namespace BitterCMS.CMSSystem
{
    public sealed class CMSRuntimer : BaseInteraction, IInitInRoot
    {
        public override Priority PriorityInteraction { get => Priority.High; }

        private static HashSet<CMSPresenter> _cmsPresenters;

        public void Init()
        {
            _cmsPresenters = new HashSet<CMSPresenter>();
            FindAll();
        }
        
        public static T GetPresenter<T>() where T : CMSPresenter
        {
            foreach (var element in _cmsPresenters)
            {
                if (element is T elementData)
                    return elementData;
            }
            throw new Exception("CMSManager not found");
        }
        
        public static CMSPresenter GetPresenterForType<TEntity>() where TEntity : CMSEntity
        {
            foreach (var presenter in _cmsPresenters)
            {
                if (presenter.IsTypeAllowed(typeof(TEntity)))   
                {
                    return presenter;
                }
            }
            throw new Exception($"No presenter found that can handle type {typeof(TEntity).Name}");
        }

        public static IReadOnlyCollection<CMSPresenter> GetAllPresenters()
        {
            return _cmsPresenters;
        }

        private static void FindAll()
        {
            var manager = ReflectionUtility.FindAllImplement<CMSPresenter>();
            foreach (var element in manager)
            {
                _cmsPresenters.Add(Activator.CreateInstance(element) as CMSPresenter);
            }
        }
    }
}
