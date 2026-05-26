using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI系统管理器。
/// 
/// 负责：
/// 1. 打开UI
/// 2. 关闭UI
/// 3. 管理UI实例
/// 4. 管理UI层级
/// </summary>
/// 
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    /// <summary>
    /// UI根节点。
    /// </summary>
    [SerializeField] private UIRoot uiRoot;

    /// <summary>
    /// 当前已打开UI字典。
    /// Key：UI名称
    /// Value：UI实例
    /// </summary>
    private readonly Dictionary<string, UIBase> openUIs = new();
    
    /// <summary>
    /// 缓存UI字典。
    /// Key：UI名称
    /// Value：UI实例
    /// </summary>
    private readonly Dictionary<string, UIBase> cacheUIs = new();

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// 打开UI。
    /// </summary>
    public T OpenUI<T>(T uiPrefab) where T : UIBase
    {
        string uiName = uiPrefab.name;
        /// 打开的单例直接返回
        if (uiPrefab.IsSingleton && openUIs.TryGetValue(uiName, out UIBase openedUI))
        {
            if (openedUI.uiType == UIType.Popup)
            {
                uiRoot.PopupMask.Show(openedUI);
            }
            return openedUI as T;
        }
        /// 缓存的直接打开
        if (uiPrefab.ShouldCache && cacheUIs.TryGetValue(uiName, out UIBase cachedUI))
        {
            if (cachedUI.uiType == UIType.Popup)
            {
                uiRoot.PopupMask.Show(cachedUI);
            }
            // 打开UI
            cachedUI.OnOpen();

            // 记录UI
            openUIs[uiName] = cachedUI;

            return cachedUI as T;
            
        }

        // 实例化UI
        T uiInstance = Instantiate(uiPrefab);

        // 获取对应Layer
        Transform parent = uiRoot.GetLayerRoot(uiInstance.uiLayer);

        // 挂到对应层级
        uiInstance.transform.SetParent(parent, false);

        // 打开UI
        uiInstance.OnOpen();

        if (uiInstance.uiType == UIType.Popup)
        {
            uiRoot.PopupMask.Show(uiInstance);
        }

        // 记录UI
        openUIs.Add(uiName, uiInstance);

        if (uiPrefab.ShouldCache)
        {
            cacheUIs.Add(uiName,uiInstance);
        }

        return uiInstance;

    }

    /// <summary>
    /// 关闭UI。
    /// </summary>
    public void CloseUI(UIBase ui)
    {
        if (ui == null)
        {
            return;
        }

        string uiName = ui.name.Replace("(Clone)", "");

        ui.OnClose();

        if (ui.uiType == UIType.Popup)
        {
            uiRoot.PopupMask.Hide();
        }

        openUIs.Remove(uiName);

        // 第一版先直接销毁
        if (!ui.ShouldCache)
        {
            Destroy(ui.gameObject);
        }
    }

}
