using BitterCMS.System.Serialization;
using BitterCMS.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BitterCMS.CMSSystem
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
        
        public static CMSEntity GetEntity(Type typeEntity)
        {
            if (!typeof(CMSEntity).IsAssignableFrom(typeEntity))
                throw new TypeAccessException("ERROR: type is not CMSEntity");

            return SerializerUtility.TryDeserialize(typeEntity, GetPath(typeEntity)) as CMSEntity;
        }

        public static T GetEntity<T>() where T : CMSEntity, new()
        {
            return SerializerUtility.TryDeserialize<T>(GetPath<T>());
        }

        public static string GetPath(Type typeEntity)
        {
            CheckInit();

            return AllEntity.GetValueOrDefault(typeEntity);
        }

        public static string GetPath<T>() where T : CMSEntity => GetPath(typeof(T));

        public static IReadOnlyDictionary<Type, string> GetAll()
        {
            CheckInit();

            return AllEntity;
        }

        public override void Initialize(bool forceUpdate = false)
        {
            if (_isInit && !forceUpdate)
                return;
            
            try
            {
                AllEntity.Clear();

                var allImplementEntity = ReflectionUtility.FindAllImplement<CMSEntity>()
                    .Where(entity => entity.IsDefined(typeof(SerializableAttribute), false));

                foreach (var typeEntity in allImplementEntity)
                {
                    var fullPath = GetPathToXmlEntity(typeEntity);
                    AllEntity.TryAdd(typeEntity, fullPath);
                }
                _isInit = true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database initialization failed: {ex.Message}", ex);
            }
        }
        
        private string GetPathToXmlEntity(Type typeEntity)
        {
            var directoryPath = PathUtility.GetFullPath(PathInProject.CMS_ENTITIES);
            var fileName = $"{typeEntity.Name}.xml";
            return Path.Combine(directoryPath, fileName);
        }
    }
}
