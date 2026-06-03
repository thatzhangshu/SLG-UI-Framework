using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 邮件详情弹窗。
/// </summary>
public class MailDetailPopup : UIPopupBase
{
    [Header("Buttons")]
    [SerializeField] private Button btnBack;

    [Header("Texts")]
    [SerializeField] private TMP_Text txtTitle;
    [SerializeField] private TMP_Text txtSender;
    [SerializeField] private TMP_Text txtTime;
    [SerializeField] private TMP_Text txtContent;

    private MailData currentMailData;

    public override void OnInit()
    {
        base.OnInit();

        if (btnBack != null)
        {
            btnBack.onClick.AddListener(OnClickClose);
        }
    }

    public override void OnOpen(object data)
    {
        base.OnOpen(data);

        currentMailData = data as MailData;

        RefreshView();
    }

    private void RefreshView()
    {
        if (currentMailData == null)
        {
            Debug.LogWarning("MailDetailPopup 打开失败：MailData 为空");
            return;
        }

        if (txtTitle != null)
        {
            txtTitle.text = currentMailData.title;
        }

        if (txtSender != null)
        {
            txtSender.text = $"发件人：{currentMailData.sender}";
        }

        if (txtTime != null)
        {
            txtTime.text = currentMailData.timeText;
        }

        if (txtContent != null)
        {
            txtContent.text = currentMailData.content;
        }
    }

    private void OnClickClose()
    {
        UIManager.Instance.CloseUI(this);
    }

    public override void OnDestroy()
    {
        if (btnBack != null)
        {
            btnBack.onClick.RemoveListener(OnClickClose);
        }

        base.OnDestroy();
    }
}
