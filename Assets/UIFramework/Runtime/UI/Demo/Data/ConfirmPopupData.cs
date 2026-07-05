using System;

/// <summary>
/// 通用确认弹窗数据。
/// 用于向 ConfirmPopup 传递标题、正文、按钮文本和回调。
/// </summary>
public class ConfirmPopupData
{
    public string title;
    public string content;

    public string confirmText;
    public string cancelText;

    public Action onConfirm;
    public Action onCancel;

    public bool showCancelButton;
    public bool closeOnConfirm;
    public bool closeOnCancel;

    /// <summary>
    /// 如果通过遮罩或外部 CloseUI 关闭，是否触发取消回调。
    /// 默认 false，避免非预期触发业务逻辑。
    /// </summary>
    public bool invokeCancelOnClose;

    public ConfirmPopupData(
        string title,
        string content,
        Action onConfirm = null,
        Action onCancel = null,
        string confirmText = "确认",
        string cancelText = "取消",
        bool showCancelButton = true,
        bool closeOnConfirm = true,
        bool closeOnCancel = true,
        bool invokeCancelOnClose = false)
    {
        this.title = title;
        this.content = content;
        this.confirmText = confirmText;
        this.cancelText = cancelText;
        this.onConfirm = onConfirm;
        this.onCancel = onCancel;
        this.showCancelButton = showCancelButton;
        this.closeOnConfirm = closeOnConfirm;
        this.closeOnCancel = closeOnCancel;
        this.invokeCancelOnClose = invokeCancelOnClose;
    }
}