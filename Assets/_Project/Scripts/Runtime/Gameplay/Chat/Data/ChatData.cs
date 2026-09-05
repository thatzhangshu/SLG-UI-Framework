/// <summary>
/// 聊天消息数据。
/// </summary>
public class ChatData
{
    public int messageId;
    public string playerName;
    public string timeText;
    public string content;

    public ChatData(int messageId, string playerName, string timeText, string content)
    {
        this.messageId = messageId;
        this.playerName = playerName;
        this.timeText = timeText;
        this.content = content;
    }
}