using BitterCMS.CMSSystem;
using BitterCMS.Utility.Interfaces;
using System;
using UnityEngine;

public abstract class BaseView : MonoBehaviour, IInitializable<CMSPresenter.CMSPresenterProperty>
{
    public Type ID { get => GetType(); }
    public CMSPresenter.CMSPresenterProperty Properties { get; set; }
    public void Init(CMSPresenter.CMSPresenterProperty property)
    {
        Properties ??= property;
    }
    public CMSEntity GetModel()
    {
        return Properties.Presenter.GetEntityByID(this);
    }

    public T GetModel<T>() where T : CMSEntity
    {
        return GetModel() as T;
    }

    protected void Destroy()
    {
        Properties.Presenter.DestroyEntity(this);
    }
    
}