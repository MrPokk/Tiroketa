using _Tiroketa._Script._Cms_Content.Components;
using _Tiroketa._Script._Cms_Content.Entity.Modules;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace _Tiroketa._Script._Cms_Content.View.Modueles
{
    public class ModulesDebugView : BaseView, IDragHandler, IDropHandler, IBeginDragHandler
    {
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            var transformObject = eventData.pointerDrag.transform;

            var grid = G.GetRoot<Root>().GridPresenter;
            grid.SetValueInGrid(transformObject.position, null);
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            var positionMouseToWorld = G.GetRoot<Root>().Camera.ScreenToWorldPoint(Mouse.current.position.value);
            eventData.pointerDrag.transform.position = new Vector3(positionMouseToWorld.x, positionMouseToWorld.y);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;

            var module = gameObject.GetComponent<BaseView>().GetModel<AbstractModule>();
            if (!module.HasComponent<DragComponent>())
                return;

            var grid = G.GetRoot<Root>().GridPresenter;
            var transformObject = eventData.pointerDrag.transform;
            if (!grid.IsWithinGrid(transformObject.position))
                return;

            grid.SetValueInGrid(transformObject.position, module);

            var positionInGrid = grid.ConvertingPosition(transformObject.position);
            var offset = (Vector2)transformObject.localScale / 2;
            transformObject.position = (Vector2)grid.ConvertingPosition(positionInGrid) + offset;
        }
    }
}
