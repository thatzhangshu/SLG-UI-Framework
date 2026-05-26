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

    /// <summary>
    /// pop类弹窗的管理栈
    /// </summary>
    private readonly Stack<UIBase> popupStack = new();

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
        if (uiPrefab == null)
        {
            Debug.LogError("OpenUI 失败：uiPrefab 为空");
            return null;
        }

        string uiName = uiPrefab.name;

        //打开的单例直接返回
        if (uiPrefab.IsSingleton && openUIs.TryGetValue(uiName, out UIBase openedUI))
        {
            if (openedUI.uiType == UIType.Popup)
            {
                BringPopupToTop(openedUI);
            }
            return openedUI as T;
        }
        /// 缓存的直接打开
        if (uiPrefab.ShouldCache && cacheUIs.TryGetValue(uiName, out UIBase cachedUI))
        {
            cachedUI.OnOpen();

            openUIs[uiName] = cachedUI;

            if (cachedUI.uiType == UIType.Popup)
            {
                PushPopup(cachedUI);
            }
            // 打开UI

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

        // 记录UI
        openUIs.Add(uiName, uiInstance);

        if (uiPrefab.ShouldCache)
        {
            cacheUIs.Add(uiName,uiInstance);
        }

        if (uiInstance.uiType == UIType.Popup)
        {
            PushPopup(uiInstance);
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
            RemovePopup(ui);
        }

        openUIs.Remove(uiName);

        // 第一版先直接销毁
        if (!ui.ShouldCache)
        {
            Destroy(ui.gameObject);
        }
    }

    /// <summary>
    /// 打开popup类弹窗 并且压入栈内
    /// </summary>
    public void PushPopup(UIBase popup)
    {
        if (!popup)
        {
            return;
        }
        popupStack.Push(popup);

        uiRoot.PopupMask.Show(popup);

        // 确保 Popup 显示在 Mask 上方。
        popup.transform.SetAsLastSibling();

    }

    /// <summary>
    /// 从 Popup 栈中移除指定 Popup。
    /// </summary>
    private void RemovePopup(UIBase popup)
    {
        if (popup == null)
        {
            return;
        }

        // Stack 不支持直接移除中间元素，所以这里重建一次。
        Stack<UIBase> tempStack = new Stack<UIBase>();

        while (popupStack.Count > 0)
        {
            UIBase top = popupStack.Pop();

            if (top != popup)
            {
                tempStack.Push(top);
            }
        }

        while (tempStack.Count > 0)
        {
            popupStack.Push(tempStack.Pop());
        }

        if (popupStack.Count > 0)
        {
            UIBase topPopup = popupStack.Peek();
            uiRoot.PopupMask.Show(topPopup);
            topPopup.transform.SetAsLastSibling();
        }
        else
        {
            uiRoot.PopupMask.Hide();
        }
    }

    /// <summary>
    /// 将已打开的 Popup 提到最上层 不入栈
    /// </summary>
    private void BringPopupToTop(UIBase popup)
    {
        if (popup == null)
        {
            return;
        }

        uiRoot.PopupMask.Show(popup);
        popup.transform.SetAsLastSibling();
    }

}
