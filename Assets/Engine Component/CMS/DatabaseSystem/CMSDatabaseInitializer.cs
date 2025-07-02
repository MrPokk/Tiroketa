
using Engine_Component.Utility;

namespace Engine_Component.CMSSystem
{
    public class CMSDatabaseInitializer : BaseInteraction, IInitInMain
    {
        public override Priority PriorityInteraction { get => Priority.FIRST_TASK; }

        public void Init()
        {
            UpdateDatabase();
        }

        public static void UpdateDatabase(bool forceUpdate = false)
        {
            CMSViewDatabase.FindAll(forceUpdate);
            CMSEntityDatabase.FindAll(forceUpdate);
        }
    }
}
