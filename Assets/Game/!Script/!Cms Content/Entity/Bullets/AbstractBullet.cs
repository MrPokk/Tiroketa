using Engine_Component.CMSSystem;
using Engine_Component.UnityIntegration.CMSComponent;
using Game._Script.CMSGame.Components;
using UnityEngine;

namespace Game._Script._Cms_Content.Entity.Bullets
{
    public abstract class AbstractBullet : CMSEntity
    {
        public Vector2 Direction { get; private set; }
        public float Speed { get; private set; }

        protected void InitComponents(
            ViewComponent.ViewProperty viewProperty,
            DamageComponent.DamageProperty damageProperty,
            MoveComponent.MoveProperty moveProperty
        )
        {
            AddComponent(out ViewComponent _).Init(viewProperty);
            AddComponent(out DamageComponent _).Init(damageProperty);
            AddComponent(out MoveComponent _).Init(moveProperty);

            Speed = moveProperty.Speed;
        }

        public void Init(Transform gunTransform)
        {
            SetDirection(gunTransform);
        }
        

        private void SetDirection(Transform gunTransform)
        {
            if (gunTransform != null)
            {
                var angle = gunTransform.eulerAngles.z;
                Direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            }
            
            var view = GetView();
            view.transform.position = gunTransform.position;
            
            var bulletAngle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Deg2Rad;
            view.transform.rotation = Quaternion.AngleAxis(bulletAngle, Vector3.forward);
        }
    }
}
