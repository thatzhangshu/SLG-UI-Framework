using UnityEngine;

/// <summary>
/// MailDataManager 测试脚本。
/// Day 1 测试完成后可以删除或禁用。
/// </summary>
public class MailDataManagerTest : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.AddListener(GameEvent.MailListChanged, OnMailListChanged);

        EventManager.AddListener<int>(
            GameEvent.MailUnreadCountChanged,
            OnMailUnreadCountChanged
        );

        EventManager.AddListener<int>(
            GameEvent.MailRead,
            OnMailRead
        );

        EventManager.AddListener<int>(
            GameEvent.MailDeleted,
            OnMailDeleted
        );
    }

    private void Start()
    {
        Debug.Log("[MailDataManagerTest] Start Test");

        MailDataManager.DebugPrint();

        MailDataManager.MarkMailRead(1001);

        MailDataManager.DeleteMail(1002);

        MailDataManager.DebugPrint();
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(GameEvent.MailListChanged, OnMailListChanged);

        EventManager.RemoveListener<int>(
            GameEvent.MailUnreadCountChanged,
            OnMailUnreadCountChanged
        );

        EventManager.RemoveListener<int>(
            GameEvent.MailRead,
            OnMailRead
        );

        EventManager.RemoveListener<int>(
            GameEvent.MailDeleted,
            OnMailDeleted
        );
    }

    private void OnMailListChanged()
    {
        Debug.Log("[MailDataManagerTest] Event Received: MailListChanged");
    }

    private void OnMailUnreadCountChanged(int unreadCount)
    {
        Debug.Log($"[MailDataManagerTest] Event Received: MailUnreadCountChanged = {unreadCount}");
    }

    private void OnMailRead(int mailId)
    {
        Debug.Log($"[MailDataManagerTest] Event Received: MailRead. MailId = {mailId}");
    }

    private void OnMailDeleted(int mailId)
    {
        Debug.Log($"[MailDataManagerTest] Event Received: MailDeleted. MailId = {mailId}");
    }
}