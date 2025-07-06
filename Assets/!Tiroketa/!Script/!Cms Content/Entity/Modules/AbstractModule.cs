using _Tiroketa._Script._Cms_Content.Components;
using BitterCMS.CMSSystem;
using BitterCMS.UnityIntegration.CMSComponent;

namespace _Tiroketa._Script._Cms_Content.Entity.Modules
{
    public class AbstractModule : CMSEntity
    {
        protected AbstractModule(out ViewComponent viewComponent)
        {
            viewComponent = AddComponent(out ViewComponent _);
            AddComponent(out DragComponent _);
        }
    }
}
