using BitterCMS.Utility.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BitterCMS.CMSSystem
{
    public class CMSComponentDatabase : CMSDatabase
    {
        private readonly static Dictionary<string, Type> ComponentTypesCache = new Dictionary<string, Type>();
      
        private static bool _isInit;
        private static void CheckInit()
        {
            if (!_isInit)
                new CMSComponentDatabase().Initialize();
        }

        public static Type GetTypeByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            CheckInit();

            return ComponentTypesCache.GetValueOrDefault(name);
        }

        public override void Initialize(bool forceUpdate = false)
        {
            if (_isInit && !forceUpdate)
                return;

            try
            {
                var componentTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .Where(t => !t.IsAbstract && typeof(IEntityComponent).IsAssignableFrom(t));

                foreach (var type in componentTypes)
                {
                    ComponentTypesCache.TryAdd(type.Name, type);
                }
                _isInit = true;
            }
            catch (ReflectionTypeLoadException ex)
            {
                throw new InvalidOperationException(
                    "Component type loading failed. Loader exceptions: " +
                    string.Join(", ", ex.LoaderExceptions.Select(e => e.Message)), ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Database initialization failed: {ex.Message}", ex);
            }

        }

        public static IReadOnlyDictionary<string, Type> GetAllComponentTypes()
        {
            CheckInit();

            return new Dictionary<string, Type>(ComponentTypesCache);
        }
        
    }
}
