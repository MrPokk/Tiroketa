using BitterCMS.CMSSystem;
using BitterCMS.Utility.Interfaces;
using UnityEngine;

namespace Game._Script.CMSGame.Components
{
    public class InsideItemComponent : IEntityComponent
    {
        public CMSEntity ContainItem { get; private set; }
        public void GiveItem<TEntity>() where TEntity : CMSEntity, new()
        {
            CMSRuntimer.GetPresenterForType<TEntity>().SpawnEntity(typeof(TEntity)).GetModel();
        }

        public void GiveItem<TEntity>(Transform parent) where TEntity : CMSEntity, new()
        {
            ContainItem = CMSRuntimer.GetPresenterForType<TEntity>().SpawnEntity(typeof(TEntity)).GetModel();
            ContainItem.Properties.Presenter.SetParent(ContainItem.GetView(), parent);
        }
    }

}
