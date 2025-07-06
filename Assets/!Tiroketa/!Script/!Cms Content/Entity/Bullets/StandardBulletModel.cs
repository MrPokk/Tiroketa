using BitterCMS.CMSSystem;
using Game._Script._Cms_Content.View.Bullets;
using System;

namespace Game._Script._Cms_Content.Entity.Bullets
{
    [Serializable]
    public class StandardBulletModel : AbstractBullet
    {
        public StandardBulletModel() : base(out var view)
        {
            view.Init(new(CMSViewDatabase.Get<StandardBulletView>()));
        }
    }
}
