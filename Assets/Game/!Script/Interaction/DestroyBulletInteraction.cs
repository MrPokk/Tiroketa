using Engine_Component.CMSSystem;
using Game._Script._Cms_Content.Components;
using Game._Script.Presenters;
using System.Collections.Generic;

namespace Game._Script.Interaction
{
    public class DestroyBulletInteraction : BaseInteraction, IEnterInUpdate
    {
        public void Update(float timeDelta)
        {
            var bulletPresenter = CMS.GetPresenter<BulletPresenter>();
            var bullets = bulletPresenter.GetModelEntities();
            var entitiesToDestroy = new List<CMSEntity>();

            foreach (var entity in bullets)
            {
                entity.GetComponent(out DestroyTimeComponent destroyTimeComponent);
                if (destroyTimeComponent == null)
                    continue;

                destroyTimeComponent.Properties.Decrease(timeDelta);

                if (destroyTimeComponent.Properties.TotalTime <= 0f)
                {
                    entitiesToDestroy.Add(entity);
                }
            }

            foreach (var entity in entitiesToDestroy)
            {
                CMS.GetPresenter<BulletPresenter>().DestroyEntity(entity.GetView());
            }
        }
    }
}
