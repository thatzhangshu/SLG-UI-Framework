using TMPro;
using UnityEngine;

/// <summary>
/// 单条聊天消息 Item。
/// </summary>
public class ChatMessageItem : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TMP_Text txtPlayerName;
    [SerializeField] private TMP_Text txtTime;
    [SerializeField] private TMP_Text txtContent;

    private ChatData chatData;

    public void SetData(ChatData data)
    {
        chatData = data;
        RefreshView();
    }

    private void RefreshView()
    {
        if (chatData == null)
        {
            return;
        }

        if (txtPlayerName != null)
        {
            txtPlayerName.text = chatData.playerName;
        }

        if (txtTime != null)
        {
            txtTime.text = chatData.timeText;
        }

        if (txtContent != null)
        {
            txtContent.text = chatData.content;
        }
    }
}