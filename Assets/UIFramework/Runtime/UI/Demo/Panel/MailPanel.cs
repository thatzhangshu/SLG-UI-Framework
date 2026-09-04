using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 邮件面板。
/// 
/// 职责：
/// 1. 显示邮件列表
/// 2. 监听邮件数据变化事件
/// 3. 响应邮件点击
/// 
/// 注意：
/// MailPanel 不保存真实邮件数据，
/// 真实数据由 MailDataManager 管理。
/// </summary>
public class MailPanel : UIPanelBase
{
    [Header("Popup")]
    [SerializeField] private MailDetailPopup mailDetailPopupPrefab;

    [Header("Mail List")]
    [SerializeField] private Transform mailContentRoot;
    [SerializeField] private MailItem mailItemPrefab;

    [Header("Buttons")]
    [SerializeField] private Button btnBack;
    
    private readonly List<MailItem> itemList = new List<MailItem>();

    public override void OnOpen()
    {
        base.OnOpen();

        EventManager.AddListener(GameEvent.MailListChanged, OnMailListChanged);

        BindButtons();
        RefreshMailList();
    }

    private void BindButtons()
    {
        if (btnBack != null)
        {
            btnBack.onClick.AddListener(OnClickBack);
        }

    }

    private void OnClickBack()
    {
        UIManager.Instance.Back();
    }

    protected override void OnDispose()
    {
        if (btnBack != null)
        {
            btnBack.onClick.RemoveListener(OnClickBack);
        }

        base.OnDispose();
    }

    public override void OnClose()
    {
        EventManager.RemoveListener(GameEvent.MailListChanged, OnMailListChanged);

        ClearMailItems();

        base.OnClose();
    }

    private void OnMailListChanged()
    {
        Debug.Log("[MailPanel] MailListChanged received.");

        RefreshMailList();
    }

    private void RefreshMailList()
    {
        ClearMailItems();

        if (mailContentRoot == null)
        {
            Debug.LogError("[MailPanel] mailContentRoot is null.");
            return;
        }

        if (mailItemPrefab == null)
        {
            Debug.LogError("[MailPanel] mailItemPrefab is null.");
            return;
        }

        List<MailData> mails = MailDataManager.GetMailList();

        for (int i = 0; i < mails.Count; i++)
        {
            MailItem item = Instantiate(mailItemPrefab, mailContentRoot);
            item.SetData(mails[i], OnClickMailItem);

            itemList.Add(item);
        }

        Debug.Log($"[MailPanel] RefreshMailList. Count = {mails.Count}");
    }

    private void ClearMailItems()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] != null)
            {
                Destroy(itemList[i].gameObject);
            }
        }

        itemList.Clear();
    }

    private void OnClickMailItem(MailData mailData)
    {
        if (mailData == null)
        {
            return;
        }

        Debug.Log($"[MailPanel] Click mail. MailId = {mailData.MailId}, Title = {mailData.Title}");

        if (mailDetailPopupPrefab == null)
        {
            Debug.LogError("[MailPanel] mailDetailPopupPrefab is null.");
            return;
        }

        MailDetailPopup popup = UIManager.Instance.OpenUI(mailDetailPopupPrefab);

        if (popup == null)
        {
            Debug.LogError("[MailPanel] Open MailDetailPopup failed.");
            return;
        }

        popup.SetData(mailData);
    }
}