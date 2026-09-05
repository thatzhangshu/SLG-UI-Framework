using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 通用确认弹窗。
/// 支持标题、正文、确认按钮、取消按钮和回调。
/// </summary>
public class ConfirmPopup : UIPopupBase
{
    [Header("Texts")]
    [SerializeField] private TMP_Text txtTitle;
    [SerializeField] private TMP_Text txtContent;
    [SerializeField] private TMP_Text txtConfirm;
    [SerializeField] private TMP_Text txtCancel;

    [Header("Buttons")]
    [SerializeField] private Button btnConfirm;
    [SerializeField] private Button btnCancel;
    [SerializeField] private Button btnClose;

    private ConfirmPopupData currentData;

    /// <summary>
    /// 是否已经由确认 / 取消按钮产生明确结果。
    /// 用于避免 OnClose 中重复触发回调。
    /// </summary>
    private bool hasResult;

    public override void OnInit()
    {
        base.OnInit();

        if (btnConfirm != null)
        {
            btnConfirm.onClick.AddListener(OnClickConfirm);
        }

        if (btnCancel != null)
        {
            btnCancel.onClick.AddListener(OnClickCancel);
        }

        if (btnClose != null)
        {
            btnClose.onClick.AddListener(OnClickCancel);
        }
    }

    public override void OnOpen(object data)
    {
        base.OnOpen(data);

        hasResult = false;

        if (data is ConfirmPopupData confirmData)
        {
            currentData = confirmData;
        }
        else if (data is string content)
        {
            currentData = new ConfirmPopupData("提示", content);
        }
        else
        {
            currentData = new ConfirmPopupData("提示", string.Empty);
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
            txtTitle.text = currentData.title;
        }

        if (txtContent != null)
        {
            txtContent.text = currentData.content;
        }

        if (txtConfirm != null)
        {
            txtConfirm.text = currentData.confirmText;
        }

        if (txtCancel != null)
        {
            txtCancel.text = currentData.cancelText;
        }

        if (btnCancel != null)
        {
            btnCancel.gameObject.SetActive(currentData.showCancelButton);
        }
    }

    private void OnClickConfirm()
    {
        if (currentData == null)
        {
            UIManager.Instance.CloseUI(this);
            return;
        }

        hasResult = true;

        currentData.onConfirm?.Invoke();

        if (currentData.closeOnConfirm)
        {
            UIManager.Instance.CloseUI(this);
        }
    }

    private void OnClickCancel()
    {
        if (currentData == null)
        {
            UIManager.Instance.CloseUI(this);
            return;
        }

        hasResult = true;

        currentData.onCancel?.Invoke();

        if (currentData.closeOnCancel)
        {
            UIManager.Instance.CloseUI(this);
        }
    }

    public override void OnClose()
    {
        if (currentData != null && !hasResult && currentData.invokeCancelOnClose)
        {
            hasResult = true;
            currentData.onCancel?.Invoke();
        }

        currentData = null;

        base.OnClose();
    }

    protected override void OnDispose()
    {
        if (btnConfirm != null)
        {
            btnConfirm.onClick.RemoveListener(OnClickConfirm);
        }

        if (btnCancel != null)
        {
            btnCancel.onClick.RemoveListener(OnClickCancel);
        }

        if (btnClose != null)
        {
            btnClose.onClick.RemoveListener(OnClickCancel);
        }

        base.OnDispose();
    }
}