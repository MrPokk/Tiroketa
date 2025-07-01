using Engine_Component.CMSSystem;
using Engine_Component.Utility.Interfaces;

namespace Engine_Component.UnityIntegration.BaseComponent
{
    public class ViewComponent : EntityComponent, IInitializableToArg<ViewComponent.ViewProperty>
    {
        public ViewProperty Properties { get; private set; }
        public void Init(ViewProperty property)
        {
            if (property == null)
                return;

            var viewBase = CMSView.Get(property.Original.ID);
            Properties = new ViewProperty(viewBase);
        }

        public class ViewProperty
        {
            private BaseView _current;
            public BaseView Current { get => _current; set => _current ??= value; }
            public BaseView Original { get; private set; }
            public ViewProperty(BaseView variableView)
            {
                Original = variableView;
            }
        }
    }
}
