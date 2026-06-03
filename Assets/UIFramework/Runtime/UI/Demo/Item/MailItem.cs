using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MailItem : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField]private TMP_Text senderText;
    [SerializeField]private TMP_Text timeText;
    [SerializeField] private GameObject unreadMark;
    [SerializeField] private Button btnClick;
    
    private Action<MailData> onClickCallback;
    private MailData mailData;

    public void SetData(MailData data, Action<MailData> onClick)
    {
        this.mailData = data;
        onClickCallback = onClick;
        Refreshview();
        // titleText.text = mailData.title;
        // senderText.text = mailData.sender;
        // timeText.text = mailData.timeText;
        // unreadMark.SetActive(!mailData.isRead);
    }

    public void Refreshview()
    {
        if (mailData == null) return;

        if (titleText!=null) titleText.text = mailData.title;
        if (senderText!=null) senderText.text = mailData.sender;
        if (timeText!=null) timeText.text = mailData.timeText;
        if (unreadMark!=null) unreadMark.SetActive(!mailData.isRead);
        // if (btnClick!=null) btnClick.onClick.AddListener(OnClick);
    }

    private void Awake()
    {
        if (btnClick!=null) btnClick.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (mailData == null) return;
        // if (mailData.isRead) return;
        mailData.isRead = true;
        // Refreshview();
        Debug.Log($"点击邮件：{mailData.mailId} - {mailData.title}");
        onClickCallback?.Invoke(mailData);
    }

    private void OnDestroy()
    {
        if (btnClick!=null) btnClick.onClick.RemoveListener(OnClick);
    }


}
