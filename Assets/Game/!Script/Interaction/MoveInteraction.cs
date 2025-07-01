using Engine_Component.CMSSystem;
using Game._Script.CMSGame.Components;
using Game._Script.Presenters;

namespace Game._Script.Interaction
{
    public class MoveInteraction : BaseInteraction, IEnterInPhysicUpdate
    {
        public void PhysicUpdate(float timeDelta)
        {
            var allPresenter = CMS.GetAllPresenters();
            foreach (var presenter in allPresenter)
            {
               var movePresenter = presenter.GetEntitiesToComponent<MoveComponent>();
               foreach (var entity in movePresenter)
               {
                   entity.GetComponent(out MoveComponent _).Properties?.MoveProcess?.Invoke();
               }
            }
        }
    }
}
