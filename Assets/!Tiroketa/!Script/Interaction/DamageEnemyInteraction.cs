using _Tiroketa._Script._Cms_Content.View;
using BitterCMS.CMSSystem;
using Game._Script._Cms_Content;
using Game._Script._Cms_Content.Components;
using Game._Script.CMSGame.Components;
using Game._Script.Presenters;

namespace Game._Script.Interaction
{
    public class DamageEnemyInteraction : BaseInteraction, IEnterInStart, IColliderInteraction
    {
        private ModulesPresenter _mobPresenter;
        public void Start() => _mobPresenter = CMSRuntimer.GetPresenter<ModulesPresenter>();

        public void EnterCollider(ViewCollision source, ViewCollision collision)
        {
            if (collision.GetModel() is not PlayerModel)
                return;

            if (!source.GetModel().TryGetComponent(out DamageComponent damageComponent))
                return;
            
            var healthComponent = collision.GetModel().GetComponent<HealthComponent>();
            healthComponent?.Decrease(damageComponent.ApplyDamage);
        }
    }
}
