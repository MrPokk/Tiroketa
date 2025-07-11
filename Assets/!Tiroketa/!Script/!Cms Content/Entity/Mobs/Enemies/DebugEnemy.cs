using _Tiroketa._Script._Cms_Content.Components;
using _Tiroketa._Script._Cms_Content.Entity.Modules;
using _Tiroketa._Script._Cms_Content.View.Mobs;
using BitterCMS.CMSSystem;
using BitterCMS.UnityIntegration.CMSComponent;
using Game._Script._Cms_Content;
using Game._Script._Cms_Content.Components;
using Game._Script._Cms_Content.Entity.Mobs;
using Game._Script.CMSGame.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Tiroketa._Script._Cms_Content.Entity.Mobs.Enemies
{
    [Serializable]
    public class DebugEnemy : AbstractMob
    {
        private CMSEntity _findNearest = null;
        private float _stoppingDistance = 0.05f;

        public DebugEnemy()
        {
            AddComponent(out ViewComponent _).Init(new(CMSViewDatabase.Get<PlayerView>()));
            AddComponent(out AttackProcessComponent _).Init(new(AttackProcess));
            AddComponent(out MoveProcessComponent _).Init(new(MoveProcess));
            AddComponent(out InsideItemsComponent _).Init(new(new CMSEntity[]
            {
                new DamageModule(),
            }));

            AddComponent(out MoveComponent _);  

            AddComponent(out DamageComponent _);
            AddComponent(out HealthComponent _);
        }

        private void AttackProcess()
        { 
            //G.Coroutine.Run(AttackProcessYield());
        }

        private IEnumerator AttackProcessYield()
        {
            yield return new WaitForSeconds(2);
            GetComponent(out InsideItemsComponent _).UsingItem?
                .GetComponent(out AttackProcessComponent _).Properties?.AttackProcess?.Invoke();
        }

        private void MoveProcess(float speed)
        {
            var currentView = GetView();
            if (!currentView) return;

            if (_findNearest == null)
            {
                _findNearest = TransformUtility.FindToNearest<PlayerModel>(currentView);
                if (_findNearest == null) return;
            }

            var nearestView = _findNearest.GetView();
            if (!nearestView)
            {
                _findNearest = null;
                return;
            }

            var direction = nearestView.transform.position - currentView.transform.position;
            if (!(direction.sqrMagnitude > _stoppingDistance))
                return;

            direction.Normalize();
            currentView.transform.Translate(direction * (speed * Time.deltaTime));
        }
    }
}
