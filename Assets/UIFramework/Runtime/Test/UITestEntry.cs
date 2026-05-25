using UnityEngine;

/// <summary>
/// UI测试入口。
/// </summary>
public class UITestEntry : MonoBehaviour
{
    [SerializeField]
    private MainHUD mainHUDPrefab;
    private MainHUD openedHUD;
    private void Start()
    {
        openedHUD = UIManager.Instance.OpenUI(mainHUDPrefab);

        Invoke(nameof(CloseHUD), 3f);
    }

    private void CloseHUD()
    {
        UIManager.Instance.CloseUI(openedHUD);
    }
}