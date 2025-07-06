using _Tiroketa._Script._Cms_Content.Entity.Modules;
using BitterCMS.CMSSystem;

namespace Game._Script.Presenters
{
    public class ModulesPresenter : CMSPresenter
    {
        public ModulesPresenter() : base(typeof(AbstractModule)){}
    }
}
