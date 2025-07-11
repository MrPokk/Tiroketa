using _Tiroketa._Script._Cms_Content.View;
using BitterCMS.CMSSystem;
using Game._Script._Cms_Content.Components;
using Game._Script._Cms_Content.Entity.Bullets;
using Game._Script.Presenters;

namespace Game._Script.Interaction
{
    public class DestroyBulletInteraction : BaseInteraction, IEnterInUpdate, IColliderInteraction
    {
        public void EnterCollider(ViewCollision source, ViewCollision collision)
        {
            if (source.GetModel() is AbstractBullet bullet)
                bullet.Properties.Presenter.DestroyEntity(source);
        }

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
