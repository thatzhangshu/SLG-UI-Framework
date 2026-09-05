using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 邮件面板。
/// 
/// 职责：
/// 1. 显示邮件列表
/// 2. 监听邮件数据变化事件
/// 3. 响应邮件点击
/// 4. 通过 UIItemPool 复用 MailItem
/// </summary>
public class MailPanel : UIPanelBase
{
    [Header("Mail List")]
    [SerializeField] private Transform mailContentRoot;
    [SerializeField] private MailItem mailItemPrefab;

    [Header("Popup")]
    [SerializeField] private MailDetailPopup mailDetailPopupPrefab;

    private UIItemPool<MailItem> mailItemPool;

    public override void OnOpen()
    {
        base.OnOpen();

        EnsureMailItemPool();

        EventManager.AddListener(GameEvent.MailListChanged, OnMailListChanged);

        RefreshMailList();
    }

    public override void OnClose()
    {
        EventManager.RemoveListener(GameEvent.MailListChanged, OnMailListChanged);

        ReleaseMailItems();

        base.OnClose();
    }

    protected override void OnDispose()
    {
        if (mailItemPool != null)
        {
            mailItemPool.Clear();
            mailItemPool = null;
        }

        base.OnDispose();
    }

    private void EnsureMailItemPool()
    {
        if (mailItemPool != null)
        {
            return;
        }

        if (mailItemPrefab == null)
        {
            Debug.LogError("[MailPanel] mailItemPrefab is null.");
            return;
        }

        if (mailContentRoot == null)
        {
            Debug.LogError("[MailPanel] mailContentRoot is null.");
            return;
        }

        mailItemPool = new UIItemPool<MailItem>(mailItemPrefab, mailContentRoot);
    }

    private void OnMailListChanged()
    {
        Debug.Log("[MailPanel] MailListChanged received.");

        RefreshMailList();
    }

    private void RefreshMailList()
    {
        EnsureMailItemPool();

        if (mailItemPool == null)
        {
            return;
        }

        mailItemPool.ReleaseAll();

        List<MailData> mails = MailDataManager.GetMailList();

        for (int i = 0; i < mails.Count; i++)
        {
            MailItem item = mailItemPool.Get();

            if (item == null)
            {
                continue;
            }

            item.SetData(mails[i], OnClickMailItem);
            item.transform.SetSiblingIndex(i);
        }
        
        mailItemPool.DebugPrint("MailItemPool");

        Debug.Log($"[MailPanel] RefreshMailList. Count = {mails.Count}");
    }

    private void ReleaseMailItems()
    {
        if (mailItemPool == null)
        {
            return;
        }

        mailItemPool.ReleaseAll();

        mailItemPool.DebugPrint("MailItemPool");
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

        MailDetailPopup popup = UIManager.Instance.OpenUI<MailDetailPopup>(UIName.MailDetailPopup);

        if (popup == null)
        {
            Debug.LogError("[MailPanel] Open MailDetailPopup failed.");
            return;
        }

        popup.SetData(mailData);
    }
}