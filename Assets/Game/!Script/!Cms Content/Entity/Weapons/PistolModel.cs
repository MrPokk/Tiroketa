using Engine_Component.CMSSystem;
using Engine_Component.UnityIntegration.CMSComponent;
using Game._Script._Cms_Content.Entity.Bullets;
using Game._Script._Cms_Content.Entity.Weapons;
using Game._Script._Cms_Content.View.Weapons;
using Game._Script.CMSGame.Components;
using Game._Script.Presenters;

namespace Game._Script._Cms_Content.Entity.Weapon
{
    public class PistolModel : AbstractWeapon
    {
        public PistolModel()
        {
            AddComponent(out ViewComponent _).Init(new(CMSViewDatabase.Get<WeaponDebugView>()));
            AddComponent(out AttackComponent _).Init(new(AttackProcess));
        }
        private void AttackProcess()
        {
            var bullet = CMSRuntimer.GetPresenter<BulletPresenter>().SpawnEntity(
                typeof(StandardBulletModel), position: GetView().transform.position);
            bullet.GetModel<AbstractBullet>().Init(GetView().transform);
        }
    }
}
