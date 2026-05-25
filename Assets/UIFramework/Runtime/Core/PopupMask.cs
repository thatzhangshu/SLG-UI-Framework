using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 负责：
/// 1. 背景变暗
/// 2. 阻挡下层点击
/// 3. 点击空白关闭Popup
/// 在这里做一个就可以了 之前在率土里边都是每个UI的csb里边有一个遮罩 但是颜色并不统一
/// </summary>
public class PopupMask : MonoBehaviour
{
    [SerializeField]
    private Button maskButton;

    private UIBase currentPopup;

    /// <summary>
    /// 显示遮罩
    /// </summary>
    public void Show(UIBase popup)
    {   
        currentPopup = popup;
        gameObject.SetActive(true);
        Debug.Log($"PopupMask Show: {popup.name}");
    }

    /// <summary>
    /// 隐藏遮罩
    /// </summary>
    public void Hide()
    {
        Debug.Log("PopupMask Hide");
        currentPopup = null;
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        maskButton = GetComponent<Button>();

        if (maskButton == null)
        {
            Debug.LogError("PopupMask 缺少 Button 组件", gameObject);
            return;
        }

        maskButton.onClick.AddListener(OnMaskClick);
    }

    public void OnMaskClick()
    {
        Debug.Log("PopupMask Click");

        if (currentPopup == null)
        {
            Debug.LogWarning("PopupMask 当前没有绑定 Popup");
            return;
        }

        if (!currentPopup.CanCloseByMask)
        {
            Debug.LogWarning($"{currentPopup.name} 不允许点击遮罩关闭");
            return;
        }

        UIManager.Instance.CloseUI(currentPopup);
    }
}
