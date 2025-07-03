using Engine_Component.System.Serialization;
using Engine_Component.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Engine_Component.CMSSystem
{
    public class CMSEntityDatabase : CMSDatabase
    {
        private readonly static Dictionary<Type, string> AllEntity = new Dictionary<Type, string>();
        
        private static bool _isInit;
        private static void CheckInit()
        {
            if (!_isInit)
                new CMSEntityDatabase().Initialize();
        }
        
        public static string GetPath(Type typeEntity)
        {
            CheckInit();
            
            return AllEntity.GetValueOrDefault(typeEntity);
        }

        public static string GetPath<T>() where T : CMSEntity
        {
            CheckInit();
            
            return GetPath(typeof(T));
        }

        public static IReadOnlyDictionary<Type, string> GetAll()
        {
            CheckInit();
            
            return AllEntity;
        }

        public override void Initialize(bool forceUpdate = false)
        {
            try
            {
                AllEntity.Clear();

                var allImplementEntity = ReflectionUtility.FindAllImplement<CMSEntity>()
                    .Where(entity => entity.IsDefined(typeof(SerializableAttribute), false));

                foreach (var typeEntity in allImplementEntity)
                {
                    var directoryPath = PathUtility.GetFullPath(PathInProject.CMS_ENTITIES);
                    var fileName = $"{typeEntity.Name}.xml";
                    var fullPath = Path.Combine(directoryPath, fileName);

                    if (!File.Exists(fullPath) || forceUpdate)
                    {
                       SerializerUtility.TrySerialize(typeEntity, fullPath);

                        AllEntity.TryAdd(typeEntity, fullPath);
                    }
                    else
                    {
                        AllEntity.TryAdd(typeEntity, fullPath);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database initialization failed: {ex.Message}", ex);
            }
        }
    }
}
