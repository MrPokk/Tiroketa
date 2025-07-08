using _Tiroketa._Script._Cms_Content.Components;
using BitterCMS.CMSSystem;
using BitterCMS.UnityIntegration.CMSComponent;
using Game._Script._Cms_Content.Components;
using Game._Script.CMSGame.Components;
using UnityEngine;

namespace Game._Script._Cms_Content.Entity.Bullets
{
    public abstract class AbstractBullet : CMSEntity
    {
        private Vector2 Direction { get; set; }

        protected AbstractBullet(out ViewComponent viewComponent)
        {
            viewComponent = AddComponent(out ViewComponent _);

            AddComponent(out MoveProcessComponent _).Init(new(Move));
            AddComponent(out MoveComponent _);
            AddComponent(out DestroyTimeComponent _);

            AddComponent<ColliderComponent>();
        }

        public void Launch(Transform gunTransform)
        {
            if (!gunTransform) return;

            SetDirection(gunTransform);
            PositionView(gunTransform);
        }

        private void SetDirection(Transform gunTransform)
        {
            float angle = gunTransform.eulerAngles.z;
            Direction = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad));
        }

        private void PositionView(Transform gunTransform)
        {
            Transform viewTransform = GetView().transform;
            viewTransform.position = gunTransform.position;

            float bulletAngle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
            viewTransform.rotation = Quaternion.AngleAxis(bulletAngle, Vector3.forward);
        }

        protected virtual void Move(float speed)
        {
            if (TryGetView(out BaseView view))
                view.transform.Translate(Vector3.right * speed);
        }
    }
}
