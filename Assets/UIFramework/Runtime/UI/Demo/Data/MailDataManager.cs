using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 邮件数据管理器。
/// 
/// 职责：
/// 1. 保存邮件运行时数据
/// 2. 提供邮件查询接口
/// 3. 修改邮件状态
/// 4. 数据变化时派发事件
/// 
/// 注意：
/// MailDataManager 不负责刷新 UI，
/// 不负责显示红点，
/// 不负责打开弹窗。
/// </summary>
public static class MailDataManager
{
    private static readonly List<MailData> mailList = new List<MailData>();

    public static bool IsInitialized { get; private set; }

    /// <summary>
    /// 初始化模拟邮件数据。
    /// MVP 阶段先使用本地假数据。
    /// 后续可替换为服务器下发数据。
    /// </summary>
    public static void InitMockData()
    {
        mailList.Clear();

        mailList.Add(new MailData(
            1001,
            "系统公告",
            "欢迎进入 SLG UI Framework Demo。",
            "系统",
            "刚刚",
            false,
            false
        ));

        mailList.Add(new MailData(
            1002,
            "登录奖励",
            "今日登录奖励已发放，请及时领取。",
            "活动中心",
            "5 分钟前",
            false,
            true
        ));

        mailList.Add(new MailData(
            1003,
            "战报通知",
            "你的部队在前线取得了一场胜利。",
            "战报系统",
            "10 分钟前",
            false,
            false
        ));

        mailList.Add(new MailData(
            1004,
            "同盟邀请",
            "有同盟向你发出了加入邀请。",
            "同盟系统",
            "30 分钟前",
            true,
            false
        ));

        mailList.Add(new MailData(
            1005,
            "资源补给",
            "主城资源补给已到达。",
            "资源系统",
            "1 小时前",
            true,
            true
        ));

        IsInitialized = true;

        Debug.Log($"[MailDataManager] InitMockData complete. MailCount = {mailList.Count}, UnreadCount = {GetUnreadCount()}");

        DispatchMailListChanged();
        DispatchUnreadCountChanged();
    }

    /// <summary>
    /// 获取邮件列表。
    /// 返回拷贝列表，避免外部直接 Add / Remove 破坏内部数据。
    /// </summary>
    public static List<MailData> GetMailList()
    {
        return new List<MailData>(mailList);
    }

    /// <summary>
    /// 根据 mailId 获取指定邮件。
    /// </summary>
    public static MailData GetMail(int mailId)
    {
        for (int i = 0; i < mailList.Count; i++)
        {
            if (mailList[i].MailId == mailId)
            {
                return mailList[i];
            }
        }

        Debug.LogWarning($"[MailDataManager] GetMail failed. MailId = {mailId}");
        return null;
    }

    /// <summary>
    /// 获取未读邮件数量。
    /// </summary>
    public static int GetUnreadCount()
    {
        int count = 0;

        for (int i = 0; i < mailList.Count; i++)
        {
            if (!mailList[i].IsRead)
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>
    /// 标记邮件已读。
    /// </summary>
    public static void MarkMailRead(int mailId)
    {
        MailData mailData = GetMail(mailId);

        if (mailData == null)
        {
            return;
        }

        if (mailData.IsRead)
        {
            Debug.Log($"[MailDataManager] Mail already read. MailId = {mailId}");
            return;
        }

        mailData.MarkRead();

        Debug.Log($"[MailDataManager] MarkMailRead. MailId = {mailId}, UnreadCount = {GetUnreadCount()}");

        EventManager.Dispatch(GameEvent.MailRead, mailId);

        DispatchMailListChanged();
        DispatchUnreadCountChanged();
    }

    /// <summary>
    /// 删除邮件。
    /// </summary>
    public static void DeleteMail(int mailId)
    {
        MailData mailData = GetMail(mailId);

        if (mailData == null)
        {
            return;
        }

        bool wasUnread = !mailData.IsRead;

        mailList.Remove(mailData);

        Debug.Log($"[MailDataManager] DeleteMail. MailId = {mailId}, MailCount = {mailList.Count}");

        EventManager.Dispatch(GameEvent.MailDeleted, mailId);

        DispatchMailListChanged();

        if (wasUnread)
        {
            DispatchUnreadCountChanged();
        }
    }

    /// <summary>
    /// 清空所有邮件。
    /// </summary>
    public static void ClearAll()
    {
        mailList.Clear();

        Debug.Log("[MailDataManager] ClearAll");

        DispatchMailListChanged();
        DispatchUnreadCountChanged();
    }

    private static void DispatchMailListChanged()
    {
        EventManager.Dispatch(GameEvent.MailListChanged);
    }

    private static void DispatchUnreadCountChanged()
    {
        int unreadCount = GetUnreadCount();

        EventManager.Dispatch(GameEvent.MailUnreadCountChanged, unreadCount);
    }

    public static void DebugPrint()
    {
        Debug.Log("========== MailDataManager Debug ==========");
        Debug.Log($"MailCount = {mailList.Count}");
        Debug.Log($"UnreadCount = {GetUnreadCount()}");

        for (int i = 0; i < mailList.Count; i++)
        {
            MailData mail = mailList[i];

            Debug.Log(
                $"MailId = {mail.MailId}, " +
                $"Title = {mail.Title}, " +
                $"IsRead = {mail.IsRead}, " +
                $"HasReward = {mail.HasReward}"
            );
        }

        Debug.Log("===========================================");
    }
}