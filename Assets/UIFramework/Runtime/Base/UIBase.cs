using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIBase : MonoBehaviour
{
    public UIType uiType;
    public UILayer uiLayer;

    [Header("Animation")]
    [SerializeField] private float openAnimationDuration = 0.15f;
    [SerializeField] private float closeAnimationDuration = 0.12f;
    [SerializeField] private bool useScaleAnimation = true;

    [Header("Interaction")]
    [SerializeField] private bool blockRaycastsWhenVisible = true;
    [SerializeField] private bool interactableWhenVisible = true;

    private CanvasGroup canvasGroup;
    private Coroutine animationCoroutine;
    private Vector3 originScale;

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

    public bool IsDisposed { get; private set; }


    public virtual void OnInit()
    {   
        Debug.Log($"{name} OnInit");
        IsInitialized = true;
    }
    /// <summary>
    /// 打开无参数版本
    /// </summary>
    public virtual void OnOpen()
    {
        Debug.Log($"{name} OnOpen");
        if (!IsInitialized)
        {
            OnInit();
        }
        IsOpen = true;
        OnShow();
    }
    /// <summary>
    /// 打开有参数版本
    /// </summary>
    /// <param name="data"></param>
    public virtual void OnOpen(object data)
    {
        OnOpen();
    }

    public virtual void OnShow()
    {
        Debug.Log($"{name} OnShow");
        IsVisible = true;
        gameObject.SetActive(true); 
        // if (NeedPlayOpenAnimation)
        // {
        //     PlayOpenAnimation();
        // }
    }

    public virtual void EnsureCanvasGroup()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();

            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        if (originScale == Vector3.zero)
        {
            originScale = transform.localScale;
        }
    }

    public virtual void PlayOpenAnimation(Action onComplete = null)
    {
        EnsureCanvasGroup();

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
        
        gameObject.SetActive(true);

        IsAnimating = true;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        if (!NeedPlayOpenAnimation || openAnimationDuration <= 0f)
        {
            IsAnimating = false;
            canvasGroup.blocksRaycasts = blockRaycastsWhenVisible;
            canvasGroup.interactable = interactableWhenVisible;
            onComplete?.Invoke();
            return;
        }

        animationCoroutine = StartCoroutine(PlayOpenAnimationCoroutine(onComplete));
    }

    private IEnumerator PlayOpenAnimationCoroutine(Action onComplete)
    {
        float timer = 0f;

        canvasGroup.alpha = 0f;

        bool shouldScale =
            useScaleAnimation &&
            uiType == UIType.Popup;

        if (shouldScale)
        {
            transform.localScale = originScale * 0.95f;
        }
        else
        {
            transform.localScale = originScale;
        }

        while (timer < openAnimationDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(timer / openAnimationDuration);

            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

            if (shouldScale)
            {
                transform.localScale = Vector3.Lerp(originScale * 0.95f, originScale, t);
            }

            yield return null;
        }

        canvasGroup.alpha = 1f;
        transform.localScale = originScale;

        IsAnimating = false;
        canvasGroup.interactable = interactableWhenVisible;
        canvasGroup.blocksRaycasts = blockRaycastsWhenVisible;

        animationCoroutine = null;

        onComplete?.Invoke();
    }

    public void PlayCloseAnimation(Action onComplete)
    {
        EnsureCanvasGroup();

        if (!gameObject.activeInHierarchy)
        {
            onComplete?.Invoke();
            return;
        }

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }

        IsAnimating = true;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        if (!NeedPlayOpenAnimation || closeAnimationDuration <= 0f)
        {
            IsAnimating = false;
            onComplete?.Invoke();
            return;
        }

        animationCoroutine = StartCoroutine(PlayCloseAnimationCoroutine(onComplete));
    }

    private IEnumerator PlayCloseAnimationCoroutine(Action onComplete)
    {
        float timer = 0f;

        float startAlpha = canvasGroup.alpha;

        bool shouldScale =
            useScaleAnimation &&
            uiType == UIType.Popup;

        Vector3 startScale = transform.localScale;
        Vector3 endScale = originScale * 0.95f;

        while (timer < closeAnimationDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(timer / closeAnimationDuration);

            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t);

            if (shouldScale)
            {
                transform.localScale = Vector3.Lerp(startScale, endScale, t);
            }

            yield return null;
        }

        canvasGroup.alpha = 0f;

        if (shouldScale)
        {
            transform.localScale = originScale;
        }

        IsAnimating = false;
        animationCoroutine = null;

        onComplete?.Invoke();
    }
    
    public virtual void OnClose()
    {
        Debug.Log($"{name} OnClose");
        IsOpen = false;
        OnHide();
    }
    public virtual void OnHide()
    {
        Debug.Log($"{name} OnHide");
        IsVisible = false;
        gameObject.SetActive(false); 
    }

    /// <summary>
    /// 由 UI 框架主动调用。
    /// 保证每个 UI 实例只释放一次。
    /// </summary>
    public void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        IsDisposed = true;

        OnDispose();

        IsInitialized = false;
        IsVisible = false;
        IsOpen = false;
        IsAnimating = false;
    }

    /// <summary>
    /// 派生 UI 在这里解绑监听、停止协程、释放资源。
    /// </summary>
    protected virtual void OnDispose()
    {

    }

    /// <summary>
    /// Unity 自动调用。
    /// 只作为外部销毁、场景卸载时的安全兜底。
    /// </summary>
    protected virtual void OnDestroy()
    {
        Dispose();
    }
}