using UnityEngine;

/// <summary>
/// UI 测试入口。
/// 用于验证 HUD、Popup、PopupMask、PopupStack。
/// </summary>
public class UITestEntry : MonoBehaviour
{
    [SerializeField]
    private MainHUD mainHUDPrefab;

    [SerializeField]
    private ConfirmPopup confirmPopupPrefab;

    [SerializeField]
    private RewardPopup rewardPopupPrefab;

    private void Start()
    {
        UIManager.Instance.OpenUI(mainHUDPrefab);

        Invoke(nameof(OpenConfirmPopup), 2f);
        Invoke(nameof(OpenRewardPopup), 4f);
    }

    private void OpenConfirmPopup()
    {
        UIManager.Instance.OpenUI(confirmPopupPrefab);
    }

    private void OpenRewardPopup()
    {
        UIManager.Instance.OpenUI(rewardPopupPrefab);
    }
}