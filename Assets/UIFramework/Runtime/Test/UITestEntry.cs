using UnityEngine;

/// <summary>
/// UI 测试入口。
/// 当前阶段只负责打开 MainHUD。
/// 后续具体页面由 MainHUD 按钮触发。
/// </summary>
public class UITestEntry : MonoBehaviour
{
    [SerializeField]
    private MainHUD mainHUDPrefab;

    private void Start()
    {
        UIManager.Instance.OpenUI(mainHUDPrefab);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.Back();
        }
    }
}