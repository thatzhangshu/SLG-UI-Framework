using UnityEngine;

/// <summary>
/// EventManager 测试脚本。
/// 测试完成后可以删除。
/// </summary>
public class EventTest : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.AddListener(GameEvent.MailListChanged, OnMailListChanged);

        EventManager.AddListener<int>(
            GameEvent.MailUnreadCountChanged,
            OnMailUnreadCountChanged
        );
    }

    private void Start()
    {
        EventManager.Dispatch(GameEvent.MailListChanged);

        EventManager.Dispatch(GameEvent.MailUnreadCountChanged, 3);

        EventManager.DebugPrint();
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(GameEvent.MailListChanged, OnMailListChanged);

        EventManager.RemoveListener<int>(
            GameEvent.MailUnreadCountChanged,
            OnMailUnreadCountChanged
        );
    }

    private void OnMailListChanged()
    {
        Debug.Log("[EventTest] MailListChanged received.");
    }

    private void OnMailUnreadCountChanged(int count)
    {
        Debug.Log($"[EventTest] MailUnreadCountChanged received. Count = {count}");
    }
}