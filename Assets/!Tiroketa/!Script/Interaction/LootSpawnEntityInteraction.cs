using _Tiroketa._Script._Cms_Content.Components;
using BitterCMS.CMSSystem;
using BitterCMS.Utility;
using Game._Script._Cms_Content;
using Game._Script._Cms_Content.Components;
using Game._Script.CMSGame.Components;
using Game._Script.Presenters;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Game._Script.Interaction
{
    public class LootSpawnEntityInteraction : BaseInteraction, IAtDestroyEntityInteraction
    {
        public void AtDestroyUpdate(CMSEntity sourceDestroyEntity)
        {
            if (!sourceDestroyEntity.TryGetComponent(out InsideItemsComponent lootComponent))
                return;

            if (lootComponent.Properties.GetLoots() == null)
                return;
            
            foreach (var loot in lootComponent.Properties.GetLoots())
            {
                var viewEntity = sourceDestroyEntity.GetView();
                if (!viewEntity)
                    break;
                
                if (loot == null)
                    continue;

                CMSRuntimer.GetPresenter<ModulesPresenter>()
                    .SpawnFromDB(loot.ID, viewEntity.transform.position);
            }
        }
    }
}
