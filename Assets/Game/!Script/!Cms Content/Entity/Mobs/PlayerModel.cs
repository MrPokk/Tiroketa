using Engine_Component.CMSSystem;
using Engine_Component.UnityIntegration.CMSComponent;
using Game._Script._Cms_Content.Entity.Mobs;
using Game._Script._Cms_Content.View.Mobs;
using Game._Script.CMSGame.Components;
using Game.TESTCMS;
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
            AddComponent(out AttackComponent _).Init(new(AttackProcess));
            AddComponent(out MoveComponent _).Init(new(MoveProcess, 5));

            AddComponent(out ControlComponent _);
            AddComponent(out InsideItemComponent _);
        }
        private void AttackProcess()
        {
            GetComponent(out InsideItemComponent _).ContainItem?
                .GetComponent(out AttackComponent _).Properties?.AttackProcess?.Invoke();
        }
        private void MoveProcess()
        {
            GetComponent(out ControlComponent controlComponent);
            GetComponent(out MoveComponent moveComponent);
            var movementDirection = moveComponent.Properties.Speed
                                    * Time.deltaTime * controlComponent.ReadInput;

            GetView().transform.Translate(movementDirection, Space.World);
        }
    }
}
