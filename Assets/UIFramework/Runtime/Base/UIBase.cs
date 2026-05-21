using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    public UIType uiType;
    public UILayer uiLayer;

    /// <summary>
    /// 是否为单例UI。
    /// true表示同类型UI同一时间只允许存在一个实例。
    /// </summary>
    public bool IsSingleton = true;

    /// <summary>
    /// 是否关闭时缓存。
    /// true表示关闭时隐藏，不销毁。
    /// </summary>
    public bool ShouldCache = true;
    public bool CanCloseByMask = true;
    public bool NeedPlayOpenAnimation = true;
    public bool IsInitialized { get; private set; }

    public bool IsVisible { get; private set; }

    public bool IsAnimating { get; private set; }

    public bool IsOpen { get; private set; }
    
    public virtual void OnInit()
    {
        IsInitialized = true;
    }
    public virtual void OnOpen()
    {
        if (!IsInitialized)
        {
            OnInit();
        }
        IsOpen = true;
        OnShow();
    }
    public virtual void OnShow()
    {
        IsVisible = true;
        gameObject.SetActive(true); 
        if (NeedPlayOpenAnimation)
        {
            PlayOpenAnimation();
        }
    }
    public virtual void PlayOpenAnimation()
    {
        //TODO: 播放打开动画
    }
    public virtual void OnClose()
    {
        IsOpen = false;
        OnHide();
    }
    public virtual void OnHide()
    {
        IsVisible = false;
        gameObject.SetActive(false); 
    }
    public virtual void OnDestroy()
    {
        IsInitialized = false;
        IsVisible = false;
        IsOpen = false;
    }
}