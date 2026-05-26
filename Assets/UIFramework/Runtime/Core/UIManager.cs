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

    /// <summary>
    /// panel类界面管理栈
    /// </summary>
    private readonly Stack<UIBase> panelStack = new();

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
            if (openedUI.uiType == UIType.Panel)
            {
                PushPanel(openedUI);
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
            if (cachedUI.uiType == UIType.Panel)
            {
                PushPanel(cachedUI);
            }

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
        openUIs[uiName] = uiInstance;

        if (uiPrefab.ShouldCache)
        {
            cacheUIs[uiName] = uiInstance;
        }

        if (uiInstance.uiType == UIType.Popup)
        {
            PushPopup(uiInstance);
        }

        if (uiInstance.uiType == UIType.Panel)
        {
            PushPanel(uiInstance);
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

        if (ui.uiType == UIType.Panel)
        {
            RemovePanel(ui);
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
        RemovePopupWithoutMaskRefresh(popup);

        popupStack.Push(popup);

        uiRoot.PopupMask.Show(popup);

        // 确保 Popup 显示在 Mask 上方。
        popup.transform.SetAsLastSibling();

    }

    /// <summary>
    /// 打开panel类界面 并且压入栈内
    /// </summary>
    public void PushPanel(UIBase panel)
    {
        if (!panel)
        {
            return;
        }
        if (panelStack.Contains(panel))
        {
            return;
        }

        panelStack.Push(panel);

        Debug.Log(panel.name);

        panel.transform.SetAsLastSibling();
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
    /// 从 Panel 栈中移除指定页面。
    /// </summary>
    private void RemovePanel(UIBase panel)
    {
        if (panel == null)
        {
            return;
        }

        Stack<UIBase> tempStack = new Stack<UIBase>();

        while (panelStack.Count > 0)
        {
            UIBase top = panelStack.Pop();

            if (top != panel)
            {
                tempStack.Push(top);
            }
        }

        while (tempStack.Count > 0)
        {
            panelStack.Push(tempStack.Pop());
        }
    }

    /// <summary>
    /// 返回上一层 Panel。
    /// 第一版逻辑：关闭当前栈顶 Panel。
    /// </summary>
    public void Back()
    {
        if (panelStack.Count == 0)
        {
            Debug.Log("当前没有可返回的 Panel");
            return;
        }

        UIBase topPanel = panelStack.Peek();

        CloseUI(topPanel);
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
        RemovePopupWithoutMaskRefresh(popup);

        popupStack.Push(popup);
        uiRoot.PopupMask.Show(popup);
        popup.transform.SetAsLastSibling();
    }

    private void RemovePopupWithoutMaskRefresh(UIBase popup)
    {
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
    }
}
