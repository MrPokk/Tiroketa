using Engine_Component.System.Serialization;
using Engine_Component.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Engine_Component.CMSSystem
{
    public static class CMSEntityDatabase
    {
        private readonly static Dictionary<Type, string> AllEntity = new Dictionary<Type, string>();

        public static IReadOnlyDictionary<Type, string> GetAll()
        {
            return AllEntity;
        }

        public static void FindAll(bool forceUpdate = false)
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
                        var pathToEntity = SerializerUtility.TrySerialize(
                            new CMSSerializerToDatabase(),
                            typeEntity,
                            directoryPath);

                        AllEntity.TryAdd(typeEntity, pathToEntity);
                    }
                    else
                    {
                        AllEntity.TryAdd(typeEntity, fullPath);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"CMSEntityDatabase initialization failed: {ex.Message}", ex);
            }
        }
    }
}
