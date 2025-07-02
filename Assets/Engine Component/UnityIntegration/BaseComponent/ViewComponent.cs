using Engine_Component.CMSSystem;
using Engine_Component.Utility.Interfaces;
using System;

namespace Engine_Component.UnityIntegration.BaseComponent
{
   
    public class ViewComponent : EntityComponent, IInitializableToArg<ViewComponent.ViewProperty>
    {
        public ViewProperty Properties { get; set; }

        public void Init(ViewProperty property)
        {
            if (property == null)
                return;

            var viewBase = CMSViewDatabase.Get(property.ViewType);
            Properties = new ViewProperty(viewBase);
        }
        
        public class ViewProperty
        {
            public Type ViewType { get; private set; }
            [NonSerialized] private BaseView _current;  
            public BaseView Current { get => _current ??= CMSViewDatabase.Get(ViewType); set => _current = value; }
            public BaseView Original => CMSViewDatabase.Get(ViewType);

            public ViewProperty() { } 

            public ViewProperty(BaseView variableView)
            {
                ViewType = variableView.ID; 
                _current = variableView;
            }
        }
    }
}
