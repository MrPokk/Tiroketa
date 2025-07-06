using _Tiroketa._Script._Cms_Content.Components;
using BitterCMS.CMSSystem;
using Game._Script.CMSGame.Components;

namespace Game._Script.Interaction
{
    public class MoveInteraction : BaseInteraction, IEnterInPhysicUpdate
    {
        public void PhysicUpdate(float timeDelta)
        {
            var allPresenter = CMSRuntimer.GetAllPresenters();
            foreach (var presenter in allPresenter)
            {
               var movePresenter = presenter.GetEntitiesToComponent(typeof(MoveProcessComponent),typeof(MoveComponent));
               foreach (var entity in movePresenter)
               {
                   entity.GetComponent(out MoveProcessComponent _)?.Properties.MoveProcess?.Invoke(
                       entity.GetComponent<MoveComponent>().Speed);
               }
            }
        }
    }
}
