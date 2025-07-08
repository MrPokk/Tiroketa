using _Tiroketa._Script._Cms_Content.Components;
using BitterCMS.CMSSystem;
using Game._Script._Cms_Content;
using Game._Script.CMSGame.Components;
using Game._Script.Presenters;
using UnityEngine;

namespace Game._Script.Interaction
{
    public class LookEnemyInteraction : BaseInteraction, IEnterInPhysicUpdate, IEnterInStart
    {
        private MobPresenter _mobPresenter;

        public void Start() => _mobPresenter = CMSRuntimer.GetPresenter<MobPresenter>();
  
        public void PhysicUpdate(float timeDelta)
        {
            var entities = _mobPresenter.GetEntitiesToComponent(
                requiredComponents: new[] { typeof(InsideItemComponent) },
                excludedComponents: new[] { typeof(ControlComponent) });

            foreach (var entity in entities)
            {
                ProcessEntityLook(entity);
            }
        }

        private void ProcessEntityLook(CMSEntity entity)
        {
            if (!entity.TryGetComponent(out InsideItemComponent insideItem) || 
                insideItem.ContainItem == null||
                !insideItem.ContainItem.TryGetComponent(out LookAtComponent lookAt) ||
                !insideItem.ContainItem.TryGetView(out var itemView) ||
                !entity.TryGetView(out var entityView))
            {
                return;
            }

            var playerEntity = TransformUtility.FindToNearest<PlayerModel>(entity.GetView());
            var playerPos = playerEntity.GetView().transform.position;

            var parentPos = entityView.transform.position;
            var direction = (playerPos - parentPos).normalized;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var targetRotation = Quaternion.Euler(0, 0, angle);

            itemView.transform.SetPositionAndRotation(
                parentPos + direction * lookAt.RotationRadius,
                Quaternion.Lerp(itemView.transform.rotation, targetRotation, 
                    lookAt.RotationSpeed * Time.deltaTime)
            );
        }
    }
}
