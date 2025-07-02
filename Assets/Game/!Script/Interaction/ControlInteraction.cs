using Engine_Component.CMSSystem;
using Game._Script._Cms_Content;
using Game._Script.CMSGame.Components;
using Game._Script.Presenters;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game._Script.Interaction
{
    public class ControlInteraction : BaseInteraction, IEnterInStart, IExitInGame, IEnterInUpdate
    {
        private InputSystem _inputSystem;
        public void Start()
        {
            _inputSystem = new InputSystem();
            _inputSystem.Enable();
            
            _inputSystem.InputGameplay.attack.performed += InteractionAttack;
        }
        public void Update(float timeDelta)
        {
            InteractionMove();
        }
        private void InteractionMove()
        {
            var inputVector2 = _inputSystem.InputGameplay.move.ReadValue<Vector2>();

            var allModel = CMSRuntimer.GetPresenter<MobPresenter>().GetEntitiesToComponent<ControlComponent>();
            foreach (var entity in allModel)
            {
                entity.GetComponent(out ControlComponent _).ReadInput = inputVector2;
            }
        }

        private void InteractionAttack(InputAction.CallbackContext context)
        {
            var allModel = CMSRuntimer.GetPresenter<MobPresenter>().GetEntitiesToComponents(typeof(ControlComponent), typeof(AttackComponent));
            foreach (var entity in allModel)
            {
                entity.GetComponent(out AttackComponent _).Properties.AttackProcess.Invoke();
            }
        }
        
        public static Vector3 GetMousePositionWorld()
        {
            var pointMouse = G.GetRoot<Root>().Camera.ScreenToWorldPoint(Mouse.current.position.value);
            return new(pointMouse.x, pointMouse.y, 0);
        }
        public void Stop()
        {
            _inputSystem.InputGameplay.attack.performed -= InteractionAttack;
            _inputSystem.Disable();
            _inputSystem.Dispose();
        }
    }
}
