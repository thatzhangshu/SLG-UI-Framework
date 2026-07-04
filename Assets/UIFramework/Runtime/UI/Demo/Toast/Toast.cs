using System.Collections;
using TMPro;
using UnityEngine;


/// <summary>
/// 通用 Toast 提示。
/// </summary>
public class Toast : UIToastBase
{
    [Header("Texts")]
    [SerializeField] private TMP_Text txtMessage;

    [Header("Settings")]
    [SerializeField] private float defaultDuration = 1.5f;

    private Coroutine hideCoroutine;

    public override void OnOpen(object data)
    {
        base.OnOpen(data);

        string message = string.Empty;
        float duration = defaultDuration;

        if (data is ToastData toastData)
        {
            message = toastData.message;
            duration = toastData.duration;
        }
        else if (data is string text)
        {
            message = text;
        }

        RefreshView(message);
        RestarAutoClose(duration);
    }

    private void RefreshView(string message)
    {
        if (txtMessage != null)
        {
            txtMessage.text = message;
        }
    }

    private void RestarAutoClose(float duration)
    {
        if(hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        hideCoroutine = StartCoroutine(HideCoroutine(duration));
    }

    private IEnumerator HideCoroutine(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);

        hideCoroutine = null;

        if (UIManager.Instance)
        {
            UIManager.Instance.CloseUI(this);
        }
    }

    public override void OnClose()
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        base.OnClose();
    }

}
