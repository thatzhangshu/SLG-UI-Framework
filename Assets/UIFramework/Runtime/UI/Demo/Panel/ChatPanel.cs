using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 聊天页面。
/// 当前为空壳页面，用于验证 MainHUD 入口、PanelStack 和返回逻辑。
/// 后续会扩展频道切换、消息列表、发送输入框和对象池。
/// </summary>
public class ChatPanel : UIPanelBase
{
    [SerializeField] private Button btnBack;
    public override void OnInit()
    {
        base.OnInit();
        BindButtons();
    }

    public void BindButtons()
    {
        if (btnBack != null)
        {
            btnBack.onClick.AddListener(OnClickBack);
        }
    }

    public void OnClickBack()
    {
        UIManager.Instance.Back();
    }
}