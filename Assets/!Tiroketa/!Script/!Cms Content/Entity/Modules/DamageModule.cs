using _Tiroketa._Script._Cms_Content.View.Modueles;
using BitterCMS.CMSSystem;
using BitterCMS.Utility.Interfaces;
using Game._Script.CMSGame.Components;
using System;

namespace _Tiroketa._Script._Cms_Content.Entity.Modules
{
    [Serializable]
    public class DamageModule : AbstractModule, IEntityComponent
    {
        public DamageModule() : base(out var viewComponent)
        {
            AddComponent<DamageComponent>();
            viewComponent.Init(new(CMSViewDatabase.Get<ModulesDebugView>()));
        }
    }
}
