/// <summary>
/// 邮件数据。
/// 当前阶段使用本地假数据，后续可以替换为服务器下发数据。
/// </summary>
public class MailData
{
    /// <summary>
    /// 邮件唯一 ID。
    /// </summary>
    public int mailId;

    /// <summary>
    /// 邮件类型。
    /// </summary>
    public int mailType;

    /// <summary>
    /// 邮件标题。
    /// </summary>
    public string title;

    /// <summary>
    /// 发件人。
    /// </summary>
    public string sender;

    /// <summary>
    /// 邮件时间文本。
    /// </summary>
    public string timeText;

    /// <summary>
    /// 邮件内容。
    /// </summary>
    public string content;

    /// <summary>
    /// 是否已读。
    /// </summary>
    public bool isRead;

    /// <summary>
    /// 是否有附件。
    /// </summary>
    public bool hasAttachment;

    public MailData(int mailId, int mailType, string title, string sender, string timeText, string content, bool isRead, bool hasAttachment)
    {
        this.mailId = mailId;
        this.mailType = mailType;
        this.title = title;
        this.sender = sender;
        this.timeText = timeText;
        this.content = content;
        this.isRead = isRead;
        this.hasAttachment = hasAttachment;
    }
}
