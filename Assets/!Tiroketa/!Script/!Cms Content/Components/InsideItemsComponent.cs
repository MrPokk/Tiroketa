using BitterCMS.CMSSystem;
using BitterCMS.Utility.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Game._Script.CMSGame.Components
{
    public class InsideItemsComponent : IEntityComponent,IInitializable<InsideItemsComponent.InsideItemsComponentProperty>
    {
        
        public class InsideItemsComponentProperty : InitializableProperty
        {
            private readonly CMSEntity[] _loots;
            public InsideItemsComponentProperty(CMSEntity[] loots)
            {
                _loots = loots;
            }
            public IReadOnlyCollection<CMSEntity> GetLoots() => _loots;
        }
        
        public InsideItemsComponentProperty Properties { get; set; }
        public void Init(InsideItemsComponentProperty property)
        {
            Properties ??= property;
        }
            
        public CMSEntity UsingItem { get; private set; }
        public void GiveItem<TEntity>(Transform parent) where TEntity : CMSEntity, new()
        {
            UsingItem = CMSRuntimer.GetPresenterForType<TEntity>().SpawnEntity(typeof(TEntity)).GetModel();
            UsingItem.Properties.Presenter.SetParent(UsingItem.GetView(), parent);
        }
    }
}
