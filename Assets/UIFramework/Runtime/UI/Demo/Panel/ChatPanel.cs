using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 聊天面板。
/// 当前 Demo 版本：假数据消息列表 + 输入发送。
/// </summary>
public class ChatPanel : UIPanelBase
{
    [Header("Buttons")]
    [SerializeField] private Button btnBack;
    [SerializeField] private Button btnSend;

    [Header("Input")]
    [SerializeField] private TMP_InputField inputMessage;

    [Header("Message List")]
    [SerializeField] private ScrollRect messageScrollRect;
    [SerializeField] private Transform messageContentRoot;
    [SerializeField] private ChatMessageItem messageItemPrefab;

    private readonly List<ChatData> chatDataList = new List<ChatData>();
    private readonly List<ChatMessageItem> messageItemList = new List<ChatMessageItem>();

    private int nextMessageId = 1;

    public override void OnInit()
    {
        base.OnInit();

        BindButtons();
        GenerateMockData();
        RefreshMessageList();
    }

    private void BindButtons()
    {
        if (btnBack != null)
        {
            btnBack.onClick.AddListener(OnClickBack);
        }

        if (btnSend != null)
        {
            btnSend.onClick.AddListener(OnClickSend);
        }
    }

    private void GenerateMockData()
    {
        chatDataList.Clear();

        for (int i = 0; i < 20; i++)
        {
            ChatData data = new ChatData(
                nextMessageId++,
                $"玩家{i + 1}",
                "20:30",
                "20:30"
                // $"这是第 {i + 1} 条世界频道测试消息，用于验证 ChatPanel 的 ScrollView 动态刷新。"
            );

            chatDataList.Add(data);
        }
    }

    private void RefreshMessageList()
    {
        ClearMessageItems();

        foreach (ChatData data in chatDataList)
        {
            ChatMessageItem item = Instantiate(messageItemPrefab, messageContentRoot);
            item.SetData(data);
            messageItemList.Add(item);
        }

        StartCoroutine(ScrollToBottom());
    }

    private void ClearMessageItems()
    {
        foreach (ChatMessageItem item in messageItemList)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }

        messageItemList.Clear();
    }

    private void OnClickSend()
    {
        if (inputMessage == null)
        {
            return;
        }

        string content = inputMessage.text.Trim();

        if (string.IsNullOrEmpty(content))
        {
            Debug.Log("聊天内容为空");
            return;
        }

        ChatData data = new ChatData(
            nextMessageId++,
            "我",
            "现在",
            content
        );

        chatDataList.Add(data);

        ChatMessageItem item = Instantiate(messageItemPrefab, messageContentRoot);
        item.SetData(data);
        messageItemList.Add(item);

        inputMessage.text = string.Empty;

        StartCoroutine(ScrollToBottom());
    }

    private IEnumerator ScrollToBottom()
    {
        yield return null;

        Canvas.ForceUpdateCanvases();

        if (messageScrollRect != null)
        {
            messageScrollRect.verticalNormalizedPosition = 0f;
        }
    }

    private void OnClickBack()
    {
        UIManager.Instance.Back();
    }

    public override void OnDestroy()
    {
        if (btnBack != null)
        {
            btnBack.onClick.RemoveListener(OnClickBack);
        }

        if (btnSend != null)
        {
            btnSend.onClick.RemoveListener(OnClickSend);
        }

        base.OnDestroy();
    }
}