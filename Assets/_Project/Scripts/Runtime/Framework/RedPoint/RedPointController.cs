using UnityEngine;

/// <summary>
/// 红点业务控制器。
///
/// 职责：
/// 1. 监听业务事件
/// 2. 将业务事件转换为红点节点变化
/// 3. 不直接刷新 UI
/// 4. 不保存业务数据
///
/// RedPointController 是 EventManager 和 RedPointManager 之间的桥接层。
/// </summary>
public static class RedPointController
{
    public static bool IsInitialized { get; private set; }

    public static void Initialize()
    {
        if (IsInitialized)
        {
            Debug.LogWarning("[RedPointController] Already initialized.");
            return;
        }

        RegisterEvents();

        IsInitialized = true;

        Debug.Log("[RedPointController] Initialized.");
    }

    public static void Shutdown()
    {
        if (!IsInitialized)
        {
            return;
        }

        UnregisterEvents();

        IsInitialized = false;

        Debug.Log("[RedPointController] Shutdown.");
    }

    private static void RegisterEvents()
    {
        EventManager.AddListener<int>(
            GameEvent.MailUnreadCountChanged,
            OnMailUnreadCountChanged
        );

        // TODO：后续扩展
        // EventManager.AddListener<int>(GameEvent.HeroUpgradeableChanged, OnHeroUpgradeableChanged);
        // EventManager.AddListener<int>(GameEvent.ActivityRewardChanged, OnActivityRewardChanged);
        // EventManager.AddListener<int>(GameEvent.ChatUnreadCountChanged, OnChatUnreadCountChanged);
    }

    private static void UnregisterEvents()
    {
        EventManager.RemoveListener<int>(
            GameEvent.MailUnreadCountChanged,
            OnMailUnreadCountChanged
        );

        // TODO：后续扩展时同步移除监听
    }

    private static void OnMailUnreadCountChanged(int unreadCount)
    {
        Debug.Log($"[RedPointController] Mail unread count changed = {unreadCount}");

        RedPointManager.SetCount(RedPointKey.MailUnread, unreadCount);
    }

    /// <summary>
    /// 主动刷新所有红点。
    /// 用于初始化完成后同步一次当前数据状态。
    /// </summary>
    public static void RefreshAll()
    {
        RefreshMailRedPoint();

        // TODO：后续扩展
        // RefreshHeroRedPoint();
        // RefreshActivityRedPoint();
        // RefreshChatRedPoint();
    }

    private static void RefreshMailRedPoint()
    {
        int unreadCount = MailDataManager.GetUnreadCount();

        RedPointManager.SetCount(RedPointKey.MailUnread, unreadCount);

        Debug.Log($"[RedPointController] RefreshMailRedPoint. UnreadCount = {unreadCount}");
    }
}