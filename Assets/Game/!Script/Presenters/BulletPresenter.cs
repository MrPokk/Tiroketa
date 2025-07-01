using Engine_Component.CMSSystem;
using Game._Script._Cms_Content.Entity.Bullets;

namespace Game._Script.Presenters
{
    public class BulletPresenter : CMSPresenter
    {
        public BulletPresenter() : base(typeof(AbstractBullet)) { }
    }
}
