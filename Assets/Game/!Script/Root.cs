using Engine_Component.CMSSystem;
using Engine_Component.UnityIntegration;
using Game._Script._Cms_Content;
using Game._Script._Cms_Content.Entity.Weapon;
using Game._Script.CMSGame.Components;
using Game._Script.GridComponent;
using Game._Script.Presenters;
using UnityEngine;

public class Root : RootMonoBehavior
{
    public Camera Camera { get; private set; }

    [SerializeField] private GridInspector _gridInspector;

    protected override void PreGStart()
    {
        Camera = Camera.main;
    }
    protected override void GStart()
    {

        var gridPresenter = new GridPresenter(_gridInspector);
        
        var player = CMSRuntimer.GetPresenter<MobPresenter>().SpawnEntity(typeof(PlayerModel));
        var playerNew = CMSRuntimer.GetPresenter<MobPresenter>().SpawnEntity(typeof(PlayerModel));
        player.GetModel().GetComponent(out InsideItemComponent _).GiveItem<PistolModel>(player.transform);
    }
}
