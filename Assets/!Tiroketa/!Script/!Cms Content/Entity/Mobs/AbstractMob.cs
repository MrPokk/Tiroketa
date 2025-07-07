using _Tiroketa._Script._Cms_Content.Components;
using BitterCMS.CMSSystem;

namespace Game._Script._Cms_Content.Entity.Mobs
{
    public abstract class AbstractMob : CMSEntity
    {
        protected AbstractMob()
        {
            AddComponent<ColliderComponent>();
        }
    }
}
