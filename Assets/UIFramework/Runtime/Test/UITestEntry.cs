using UnityEngine;

/// <summary>
/// UI 测试入口。
/// 用于验证 HUD、Panel、Popup 的基础流程。
/// </summary>
public class UITestEntry : MonoBehaviour
{
    [SerializeField]
    private MainHUD mainHUDPrefab;

    [SerializeField]
    private MailPanel mailPanelPrefab;

    [SerializeField]
    private ConfirmPopup confirmPopupPrefab;

    private void Start()
    {
        UIManager.Instance.OpenUI(mainHUDPrefab);

        Invoke(nameof(OpenMailPanel), 2f);
        Invoke(nameof(OpenConfirmPopup), 4f);
        Invoke(nameof(BackPanel), 7f);
    }

    private void OpenMailPanel()
    {
        UIManager.Instance.OpenUI(mailPanelPrefab);
    }

    private void OpenConfirmPopup()
    {
        UIManager.Instance.OpenUI(confirmPopupPrefab);
    }

    private void BackPanel()
    {
        UIManager.Instance.Back();
    }
}