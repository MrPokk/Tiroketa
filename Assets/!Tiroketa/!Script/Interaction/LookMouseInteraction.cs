using _Tiroketa._Script._Cms_Content.Components;
using BitterCMS.CMSSystem;
using Game._Script._Cms_Content;
using Game._Script.CMSGame.Components;
using Game._Script.Presenters;
using UnityEngine;

namespace Game._Script.Interaction
{
    public class LookMouseInteraction : BaseInteraction, IEnterInStart , IEnterInPhysicUpdate
    {
        private MobPresenter _mobPresenter;
        private Vector3 _mouseWorldPos;

        public void Start() => _mobPresenter = CMSRuntimer.GetPresenter<MobPresenter>();
        
        public void PhysicUpdate(float timeDelta)
        {
            _mouseWorldPos = ControlInteraction.GetMousePositionWorld();

            var entities = _mobPresenter.GetEntitiesToComponent(typeof(ControlComponent), typeof(InsideItemComponent));

            foreach (var entity in entities)
            {
                ProcessEntityLook(entity);
            }
        }

        private void ProcessEntityLook(CMSEntity entity)
        {
            if (!entity.TryGetComponent(out InsideItemComponent insideItem) ||
                insideItem.ContainItem == null ||
                !insideItem.ContainItem.TryGetComponent(out LookAtComponent lookAt) ||
                !insideItem.ContainItem.TryGetView(out var itemView) ||
                !entity.TryGetView(out var entityView))
            {
                return;
            }

            var parentPos = entityView.transform.position;
            var direction = (_mouseWorldPos - parentPos).normalized;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var targetRotation = Quaternion.Euler(0, 0, angle);

            itemView.transform.SetPositionAndRotation(
                parentPos + direction * lookAt.RotationRadius,
                Quaternion.Lerp(itemView.transform.rotation, targetRotation, 
                    lookAt.RotationSpeed * Time.deltaTime * 5f)
            );
        }
    }
}
