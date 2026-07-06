using UnityEngine;

/// <summary>
/// 游戏启动入口。
/// 负责初始化全局系统，并打开初始 UI。
/// </summary>
public class GameLauncher : MonoBehaviour
{
    [Header("Initial UI")]
    [SerializeField] private MainHUD mainHUDPrefab;

    private void Awake()
    {
        InitCoreSystems();
    }

    private void Start()
    {
        EnterGame();
    }

    private void InitCoreSystems()
    {
        ConfigManager.LoadAll();

        Debug.Log("[GameLauncher] Core systems initialized.");
    }

    private void EnterGame()
    {
        InitRedPointState();

        if (UIManager.Instance != null && mainHUDPrefab != null)
        {
            UIManager.Instance.OpenUI(mainHUDPrefab);
        }
    }

    private void InitRedPointState()
    {
        // RedPointManager.SetRedPoint(RedPointKey.Mail, true);
        // RedPointManager.SetRedPoint(RedPointKey.Hero, true);
        // RedPointManager.SetRedPoint(RedPointKey.Activity, true);
    }
}