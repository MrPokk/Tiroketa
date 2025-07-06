using BitterCMS.CMSSystem;
using BitterCMS.Utility.Interfaces;
using System.Linq;
using UnityEngine;

public static class TransformUtility
{
    public static CMSEntity FindToNearestComponent<TComponent>(BaseView fromModel) where TComponent : class, IEntityComponent
    {
        if (fromModel == null) return null;

        CMSEntity nearestEntity = null;
        var nearestDistance = float.MaxValue;
        Vector2 fromModelPosition = fromModel.transform.position;

        foreach (var entity in GetAllEntities())
        {
            var distance = Vector2.Distance(fromModelPosition, entity.Key.transform.position);
            
            if (distance < nearestDistance && entity.Value.HasComponent(typeof(TComponent)))
            {
                nearestDistance = distance;
                nearestEntity = entity.Value;
            }
        }

        return nearestEntity;
    }

    public static CMSEntity FindToNearest<TEntity>(BaseView fromModel) where TEntity : CMSEntity
    {
        if (!fromModel) return null;

        CMSEntity nearestEntity = null;
        var nearestDistance = float.MaxValue;
        Vector2 fromModelPosition = fromModel.transform.position;

        foreach (var entity in GetAllEntities())
        {
            if (entity.Value is not TEntity) continue;

            var distance = Vector2.Distance(fromModelPosition, entity.Key.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEntity = entity.Value;
            }
        }

        return nearestEntity;
    }

    public static CMSEntity FindToNearest(BaseView fromModel)
    {
        if (fromModel == null) return null;

        CMSEntity nearestEntity = null;
        var nearestDistance = float.MaxValue;
        Vector2 fromModelPosition = fromModel.transform.position;

        foreach (var entity in GetAllEntities())
        {
            var distance = Vector2.Distance(fromModelPosition, entity.Key.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEntity = entity.Value;
            }
        }

        return nearestEntity;
    }

    private static System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<BaseView, CMSEntity>> GetAllEntities()
    {
        return CMSRuntimer.GetAllPresenters().SelectMany(cmsManager => cmsManager.GetEntities());
    }
}