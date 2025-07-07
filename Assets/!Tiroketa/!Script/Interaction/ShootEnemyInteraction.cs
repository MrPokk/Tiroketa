using BitterCMS.CMSSystem;
using Game._Script._Cms_Content;
using Game._Script.CMSGame.Components;
using Game._Script.Presenters;

namespace Game._Script.Interaction
{
    public class ShootEnemyInteraction : BaseInteraction, IEnterInUpdate, IEnterInStart
    {
        private MobPresenter _mobPresenter;
        public void Start() => _mobPresenter = CMSRuntimer.GetPresenter<MobPresenter>();
        public void Update(float timeDelta)
        {
            var entities = _mobPresenter.GetEntitiesToComponent(
                requiredComponents: new[] { typeof(AttackProcessComponent) },
                excludedComponents: new[] { typeof(ControlComponent) });

            foreach (var entity in entities)
            {
                entity.GetComponent<AttackProcessComponent>()?.Properties?.AttackProcess?.Invoke();
            }
        }
    }
}
