using Engine_Component.CMSSystem;
using Game._Script._Cms_Content.Entity.Weapon;

namespace Game._Script.Presenters
{
    public class WeaponPresenter : CMSPresenter
    {
        public WeaponPresenter() : base(typeof(PistolModel)) { }
    }
}
