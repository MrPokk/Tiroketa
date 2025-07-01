using Engine_Component.CMSSystem;
using Engine_Component.Utility.Interfaces;
using System;
using UnityEngine;

namespace Game._Script.CMSGame.Components
{
    public class InsideItemComponent : EntityComponent
    {
        public CMSEntity ContainItem { get; private set; }
        public void GiveItem<TEntity>() where TEntity : CMSEntity, new()
        {
            CMS.GetPresenterForType<TEntity>().SpawnEntity(typeof(TEntity)).GetModel();
        }

        public void GiveItem<TEntity>(Transform parent) where TEntity : CMSEntity, new()
        {
            ContainItem = CMS.GetPresenterForType<TEntity>().SpawnEntity(typeof(TEntity)).GetModel();
            ContainItem.Properties.Presenter.SetParent(ContainItem.GetView(), parent);
        }
    }

}
