using BitterCMS.Utility;
using System;

namespace BitterCMS.CMSSystem
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
            var allDatabase = ReflectionUtility.FindAllImplement<CMSDatabase>();
            foreach (var database in allDatabase)
            {
                if (Activator.CreateInstance(database) is CMSDatabase newDatabase)
                    newDatabase.Initialize(forceUpdate);
            }
        }
    }
}
