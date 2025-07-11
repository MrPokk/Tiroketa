using _Tiroketa._Script._Cms_Content.Components;
using _Tiroketa._Script._Cms_Content.View.Mobs;
using BitterCMS.CMSSystem;
using BitterCMS.UnityIntegration.CMSComponent;
using Game._Script._Cms_Content.Components;
using Game._Script._Cms_Content.Entity.Mobs;
using Game._Script.CMSGame.Components;
using System;
using UnityEngine;

namespace Game._Script._Cms_Content
{
    [Serializable]
    public class PlayerModel : AbstractMob
    {
        public PlayerModel()
        {
            AddComponent(out ViewComponent _).Init(new(CMSViewDatabase.Get<PlayerView>()));
            AddComponent(out AttackProcessComponent _).Init(new(AttackProcess));
            AddComponent(out MoveProcessComponent _).Init(new(MoveProcess));
            
            AddComponent(out HealthComponent _);
            AddComponent(out MoveComponent _);

            AddComponent(out ControlComponent _);
            AddComponent(out InsideItemsComponent _);
        }
        
        private void AttackProcess()
        {
            GetComponent(out InsideItemsComponent _).UsingItem?
                .GetComponent(out AttackProcessComponent _).Properties?.AttackProcess?.Invoke();
        }
        
        private void MoveProcess(float speed)
        {
            GetComponent(out ControlComponent controlComponent);
            var movementDirection = speed
                                    * Time.deltaTime * controlComponent.ReadInput;

            GetView().transform.Translate(movementDirection, Space.World);
        }
    }
}
