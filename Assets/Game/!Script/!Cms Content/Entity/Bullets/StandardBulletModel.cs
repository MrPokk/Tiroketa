using Engine_Component.CMSSystem;
using Game._Script._Cms_Content.View.Bullets;
using UnityEngine;

namespace Game._Script._Cms_Content.Entity.Bullets
{
    public class StandardBulletModel : AbstractBullet
    {
        public StandardBulletModel()
        {
            InitComponents(
                new(CMSView.Get<StandardBulletView>()),
                new(10),
                new(2),
                new(MoveProcess, 40));
        }
        private void MoveProcess()
        {
            GetView().transform.Translate(Direction * (Speed * Time.deltaTime));
        }
    }
}
