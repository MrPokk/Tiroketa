using _Tiroketa._Script._Cms_Content.View;
using Game._Script._Cms_Content;
using Game._Script._Cms_Content.Components;
using Game._Script.CMSGame.Components;
using UnityEngine;

namespace Game._Script.Interaction
{
    public class DamageEnemyInteraction : BaseInteraction, IColliderInteraction
    {
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
