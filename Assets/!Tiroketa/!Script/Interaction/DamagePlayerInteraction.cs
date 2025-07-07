using _Tiroketa._Script._Cms_Content.Components;
using _Tiroketa._Script._Cms_Content.Entity.Modules;
using _Tiroketa._Script._Cms_Content.View;
using BitterCMS.CMSSystem;
using Game._Script._Cms_Content;
using Game._Script._Cms_Content.Components;
using Game._Script._Cms_Content.Entity.Bullets;
using Game._Script._Cms_Content.Entity.Mobs;
using Game._Script.CMSGame.Components;
using Game._Script.GridComponent;
using Game._Script.Presenters;
using System.Collections.Generic;
using UnityEngine;

namespace Game._Script.Interaction
{
    public class DamagePlayerInteraction : BaseInteraction, IColliderInteraction, IEnterInStart
    {
        private List<AbstractModule> AllModules => G.GetRoot<Root>().GridPresenter.GetAllContains();

        private MobPresenter _mobPresenter;
        public void Start() => _mobPresenter = CMSRuntimer.GetPresenter<MobPresenter>();
        public void EnterCollider(ViewCollision source, ViewCollision collision)
        {
            if (collision.GetModel() is not AbstractBullet)
                return;   
            
            var healthComponent = source.GetModel().GetComponent<HealthComponent>();
            foreach (var module in AllModules)
            {
                module.GetComponent(out DamageComponent damageComponent);
                if (damageComponent == null)
                    continue;
                
                healthComponent.Decrease(damageComponent.ApplyDamage);
            }
        }
    }
}
