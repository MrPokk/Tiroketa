using BitterCMS.CMSSystem;
using BitterCMS.Utility.Interfaces;
using System;

namespace BitterCMS.UnityIntegration.CMSComponent
{
    public class ViewComponent : IEntityComponent, IInitializable<ViewComponent.ViewProperty>
    {
        public ViewProperty Properties { get; set; }

        public void Init(ViewProperty property)
        {
            if (property == null)
                return;

            var viewBase = CMSViewDatabase.Get(property.ViewType);
            Properties = new ViewProperty(viewBase);
        }
        
        public class ViewProperty : InitializableProperty
        {
            public Type ViewType { get; private set; }
            [NonSerialized] private BaseView _current;  
            public BaseView Current { get => _current ??= CMSViewDatabase.Get(ViewType); set => _current = value; }
            public BaseView Original => CMSViewDatabase.Get(ViewType);

            public ViewProperty(BaseView variableView)
            {
                ViewType = variableView.ID; 
                _current = variableView;
            }
        }
    }
}
