using Engine_Component.CMSSystem;
using Engine_Component.System.Serialization;
using Engine_Component.UnityIntegration;
using Game._Script._Cms_Content;
using Game._Script._Cms_Content.Entity.Weapon;
using Game._Script.CMSGame.Components;
using Game._Script.GridComponent;
using Game._Script.Presenters;
using Game.TESTCMS;
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
        //   var gridPresenter = new GridPresenter(_gridInspector);
        var test = SerializerUtility.TryDeserialize<TestEntity>(new CMSSerializer(typeof(TestEntity), CMSEntityDatabase.GetPath(typeof(TestEntity))));
        
        print(test.HasComponent<OneTestComponent>());
        print(test.HasComponent<TwoTestComponent>());
        

        var player = CMSRuntimer.GetPresenter<MobPresenter>().SpawnEntity(typeof(PlayerModel));
        player.GetModel().GetComponent(out InsideItemComponent _).GiveItem<PistolModel>(player.transform);
    }
}
