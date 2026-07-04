/// <summary>
/// 文本提示float弹窗
/// </summary>
public class ToastData
{
    public string message;
    public float duration;
    // public Action onComplete;

    public ToastData(string message, float duration = 1.5f)
    {
        this.message = message;
        this.duration = duration;
    }

}