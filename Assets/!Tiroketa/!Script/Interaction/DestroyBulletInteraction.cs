using BitterCMS.CMSSystem;
using Game._Script._Cms_Content.Components;
using Game._Script.Presenters;
using System.Collections.Generic;
using UnityEngine;

namespace Game._Script.Interaction
{
    public class DestroyBulletInteraction : BaseInteraction, IEnterInUpdate
    {
        public void Update(float timeDelta)
        {
            var bulletPresenter = CMSRuntimer.GetPresenter<BulletPresenter>();
            var bullets = bulletPresenter.GetModelEntities();

            foreach (var entity in bullets)
            {
                entity.GetComponent(out DestroyTimeComponent destroyTimeComponent);
                if (destroyTimeComponent == null)
                    continue;

                destroyTimeComponent.Decrease(timeDelta);

                if (destroyTimeComponent.TotalTime <= 0f)
                    bulletPresenter.DestroyEntity(entity.GetView());
            }
        }
    }
}
