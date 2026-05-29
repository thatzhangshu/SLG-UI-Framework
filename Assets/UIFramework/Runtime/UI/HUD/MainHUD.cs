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

    [SerializeField] private ChatPanel chatPanel;
    [SerializeField] private ActivityPanel activityPanel;

    [Header("Buttons")]
    [SerializeField] private Button btnMail;
    [SerializeField] private Button btnHero;
    [SerializeField] private Button btnChat;
    [SerializeField] private Button btnActivity;
    [SerializeField] private Button btnBack;
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

        if (btnBack != null)
        {
            btnBack.onClick.AddListener(OnClickBack);
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
        UIManager.Instance.OpenUI(chatPanel);
    }

    private void OnClickActivity()
    {
        UIManager.Instance.OpenUI(activityPanel);
    }

    private void OnClickBack()
    {
        UIManager.Instance.Back();
    }

    public override void OnDestroy()
    {
        UnbindButtons();
        base.OnDestroy();
        
    }
    private void UnbindButtons()
    {
        if (btnMail != null)
        {
            btnMail.onClick.RemoveListener(OpenMailPanel);
        }

        if (btnHero != null)
        {
            btnHero.onClick.RemoveListener(OpenHeroPanel);
        }

        if (btnChat != null)
        {
            btnChat.onClick.RemoveListener(OnClickChat);
        }

        if (btnActivity != null)
        {
            btnActivity.onClick.RemoveListener(OnClickActivity);
        }
    }
}
