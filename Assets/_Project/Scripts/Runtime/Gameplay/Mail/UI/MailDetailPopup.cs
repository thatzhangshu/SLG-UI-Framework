using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 邮件详情弹窗。
/// 
/// 职责：
/// 1. 显示邮件详情
/// 2. 打开时标记邮件已读
/// 3. 删除邮件前弹出 ConfirmPopup
/// 4. 删除成功后显示 Toast
/// 
/// 注意：
/// MailDetailPopup 不保存邮件列表，
/// 邮件真实数据仍由 MailDataManager 管理。
/// </summary>
public class MailDetailPopup : UIPopupBase
{
    [Header("UI References")]
    [SerializeField] private TMP_Text txtTitle;
    [SerializeField] private TMP_Text txtSender;
    [SerializeField] private TMP_Text txtTime;
    [SerializeField] private TMP_Text txtContent;
    [SerializeField] private GameObject rewardNode;

    [Header("Buttons")]
    [SerializeField] private Button btnClose;
    [SerializeField] private Button btnDelete;
    [SerializeField] private Button btnReceiveReward;

    private MailData currentData;

    public override void OnInit()
    {
        base.OnInit();

        if (btnClose != null)
        {
            btnClose.onClick.AddListener(OnClickClose);
        }

        if (btnDelete != null)
        {
            btnDelete.onClick.AddListener(OnClickDelete);
        }

        if (btnReceiveReward != null)
        {
            btnReceiveReward.onClick.AddListener(OnClickReceiveReward);
        }
    }

    public override void OnClose()
    {
        currentData = null;

        base.OnClose();
    }

    protected override void OnDispose()
    {
        if (btnClose != null)
        {
            btnClose.onClick.RemoveListener(OnClickClose);
        }

        if (btnDelete != null)
        {
            btnDelete.onClick.RemoveListener(OnClickDelete);
        }

        if (btnReceiveReward != null)
        {
            btnReceiveReward.onClick.RemoveListener(OnClickReceiveReward);
        }

        base.OnDispose();
    }

    public void SetData(MailData mailData)
    {
        currentData = mailData;

        if (currentData == null)
        {
            Debug.LogError("[MailDetailPopup] SetData failed. mailData is null.");
            return;
        }

        if (!currentData.IsRead)
        {
            MailDataManager.MarkMailRead(currentData.MailId);
        }

        RefreshView();
    }

    private void RefreshView()
    {
        if (currentData == null)
        {
            return;
        }

        if (txtTitle != null)
        {
            txtTitle.text = currentData.Title;
        }

        if (txtSender != null)
        {
            txtSender.text = currentData.Sender;
        }

        if (txtTime != null)
        {
            txtTime.text = currentData.TimeText;
        }

        if (txtContent != null)
        {
            txtContent.text = currentData.Content;
        }

        if (rewardNode != null)
        {
            rewardNode.SetActive(currentData.HasReward);
        }

        if (btnReceiveReward != null)
        {
            btnReceiveReward.gameObject.SetActive(currentData.HasReward);
        }
    }

    private void OnClickClose()
    {
        UIManager.Instance.CloseUI(this);
    }

    private void OnClickDelete()
    {
        if (currentData == null)
        {
            return;
        }

        int mailId = currentData.MailId;
        string title = currentData.Title;

        UIManager.Instance.ShowConfirm(
            "确认删除",
            $"是否删除邮件：{title}？",
            () =>
            {
                MailDataManager.DeleteMail(mailId);

                UIManager.Instance.CloseUI(this);

                UIManager.Instance.ShowToast("邮件已删除");
            },
            () =>
            {
                UIManager.Instance.ShowToast("已取消删除");
            }
        );
    }

    private void OnClickReceiveReward()
    {
        if (currentData == null)
        {
            return;
        }

        if (!currentData.HasReward)
        {
            UIManager.Instance.ShowToast("当前邮件没有可领取奖励");
            return;
        }

        currentData.ClearReward();

        EventManager.Dispatch(GameEvent.MailListChanged);

        RefreshView();

        UIManager.Instance.ShowToast("奖励已领取");
    }
}