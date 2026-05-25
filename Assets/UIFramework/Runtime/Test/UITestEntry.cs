using UnityEngine;

/// <summary>
/// UI 测试入口。
/// 用于验证 UIManager 打开、关闭、缓存复用流程。
/// </summary>
public class UITestEntry : MonoBehaviour
{
    [SerializeField]
    private MainHUD mainHUDPrefab;
    [SerializeField]
    private ConfirmPopup confirmPopupPrefab;

    private MainHUD openedHUD;
    private ConfirmPopup openedPopup;
    private void Start()
    {
        openedHUD = UIManager.Instance.OpenUI(mainHUDPrefab);

        Invoke(nameof(OpenPopup), 2f);
        // Invoke(nameof(ClosePopup), 5f);
        Invoke(nameof(ReopenPopup), 6f);
    }

    private void OpenPopup()
    {
        openedPopup = UIManager.Instance.OpenUI(confirmPopupPrefab);
    }

    private void ClosePopup()
    {
        UIManager.Instance.CloseUI(openedPopup);
    }

    private void ReopenPopup()
    {
        openedPopup = UIManager.Instance.OpenUI(confirmPopupPrefab);
    }
}