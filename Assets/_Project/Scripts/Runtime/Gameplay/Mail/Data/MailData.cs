/// <summary>
/// 邮件运行时数据。
/// 
/// 注意：
/// MailData 是玩家当前账号下的一封邮件数据，
/// 属于运行时数据，不是静态配置。
/// </summary>
public class MailData
{
    public int MailId { get; private set; }

    public string Title { get; private set; }

    public string Content { get; private set; }

    public string Sender { get; private set; }

    public string TimeText { get; private set; }

    public bool IsRead { get; private set; }

    public bool HasReward { get; private set; }

    public MailData(
        int mailId,
        string title,
        string content,
        string sender,
        string timeText,
        bool isRead,
        bool hasReward)
    {
        MailId = mailId;
        Title = title;
        Content = content;
        Sender = sender;
        TimeText = timeText;
        IsRead = isRead;
        HasReward = hasReward;
    }

    public void MarkRead()
    {
        IsRead = true;
    }

    public void ClearReward()
    {
        HasReward = false;
    }
}