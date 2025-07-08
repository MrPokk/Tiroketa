using _Tiroketa._Script._Cms_Content.Entity.Mobs.Enemies;
using _Tiroketa._Script._Cms_Content.Entity.Modules;
using BitterCMS.CMSSystem;
using BitterCMS.UnityIntegration;
using Game._Script._Cms_Content;
using Game._Script._Cms_Content.Entity.Weapon;
using Game._Script.CMSGame.Components;
using Game._Script.GridComponent;
using Game._Script.Interaction;
using Game._Script.Presenters;
using UnityEngine;

public class Root : RootMonoBehavior
{
    public Camera Camera { get; private set; }

    [SerializeField]
    private GridInspector GridInspector;
    public GridPresenter<AbstractModule> GridPresenter { get; private set; }

    protected override void PreGStart()
    {
        Camera = Camera.main;
    }
    protected override void GStart()
    {
        GridPresenter = new GridPresenter<AbstractModule>(GridInspector);

        var player = CMSRuntimer.GetPresenter<MobPresenter>().SpawnFromDB(typeof(PlayerModel));

        var debugEnemy = CMSRuntimer.GetPresenter<MobPresenter>().SpawnFromDB(typeof(DebugEnemy));

        CMSRuntimer.GetPresenter<ModulesPresenter>().SpawnFromDB(typeof(DamageModule));
        CMSRuntimer.GetPresenter<ModulesPresenter>().SpawnFromDB(typeof(DamageModule));

        player.GetModel().GetComponent(out InsideItemComponent _).GiveItem<PistolModel>(player.transform);
     //   debugEnemy.GetModel().GetComponent(out InsideItemComponent _).GiveItem<PistolModel>(debugEnemy.transform);
    }

    protected override void FindInteraction(Interaction interaction)
    {
        base.FindInteraction(interaction); 
        interaction.FindAll<IColliderInteraction>();
    }
}
