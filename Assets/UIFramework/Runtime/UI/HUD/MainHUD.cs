using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// SLG 主界面 HUD。
/// 负责承载常驻入口按钮，例如邮件、武将、聊天、活动。
/// </summary>
public class MainHUD : UIBase
{
    [Header("Panel Prefabs")]
    [SerializeField] private MailPanel mailPanelPrefab;
    [SerializeField] private HeroPanel heroPanelPrefab;

    [Header("Buttons")]
    [SerializeField] private Button btnMail;
    [SerializeField] private Button btnHero;
    [SerializeField] private Button btnChat;
    [SerializeField] private Button btnActivity;

    public override void OnInit()
    {
        base.OnInit();

        BindButtons();
    }

    /// <summary>
    /// 绑定主界面按钮事件。
    /// </summary>
    private void BindButtons()
    {
        if (btnMail != null)
        {
            btnMail.onClick.AddListener(OpenMailPanel);
        }

        if (btnHero != null)
        {
            btnHero.onClick.AddListener(OpenHeroPanel);
        }

        if (btnChat != null)
        {
            btnChat.onClick.AddListener(OnClickChat);
        }

        if (btnActivity != null)
        {
            btnActivity.onClick.AddListener(OnClickActivity);
        }
    }

    private void OpenMailPanel()
    {
        UIManager.Instance.OpenUI(mailPanelPrefab);
    }

    private void OpenHeroPanel()
    {
        UIManager.Instance.OpenUI(heroPanelPrefab);
    }

    private void OnClickChat()
    {
        Debug.Log("ChatPanel 尚未实现");
    }

    private void OnClickActivity()
    {
        Debug.Log("ActivityPanel 尚未实现");
    }

    
}