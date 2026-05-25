using UnityEngine;

/// <summary>
/// UI 测试入口。
/// 用于验证 UIManager 打开、关闭、缓存复用流程。
/// </summary>
public class UITestEntry : MonoBehaviour
{
    [SerializeField]
    private MainHUD mainHUDPrefab;

    private MainHUD openedHUD;

    private void Start()
    {
        openedHUD = UIManager.Instance.OpenUI(mainHUDPrefab);

        Invoke(nameof(CloseMainHUD), 3f);
        Invoke(nameof(ReopenMainHUD), 6f);
    }

    private void CloseMainHUD()
    {
        UIManager.Instance.CloseUI(openedHUD);
    }

    private void ReopenMainHUD()
    {
        openedHUD = UIManager.Instance.OpenUI(mainHUDPrefab);
    }
}