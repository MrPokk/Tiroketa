using BitterCMS.CMSSystem;
using Game._Script._Cms_Content;
using Game._Script._Cms_Content.Components;
using Game._Script.Presenters;

namespace Game._Script.Interaction
{
    public class AddEnemyInDestroyInteraction : BaseInteraction, IEnterInUpdate, IEnterInStart
    {
        private MobPresenter _mobPresenter;
        public void Start() => _mobPresenter = CMSRuntimer.GetPresenter<MobPresenter>();
        public void Update(float timeDelta)
        {
            var allEntity = _mobPresenter.GetEntitiesToComponent(
                requiredComponents:
                new[] { typeof(HealthComponent) },
                excludedComponents:
                new[] { typeof(ControlComponent) });

            foreach (var entity in allEntity)
            {
                if (!(entity.GetComponent<HealthComponent>()?.HealthCurrent <= 0))
                    continue;

                foreach (var interaction in InteractionCache<IAtDestroyEntityInteraction>.AllInteraction)
                    interaction.AtDestroyUpdate(entity);
                
                _mobPresenter.DestroyEntity(entity.GetView());
            }
        }
    }
}
