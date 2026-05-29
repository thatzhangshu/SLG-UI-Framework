using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 活动页面。
/// 当前为空壳页面，用于验证 MainHUD 入口、PanelStack 和返回逻辑。
/// 后续会扩展活动倒计时、进度条、奖励列表和 RewardPopup。
/// </summary>
public class ActivityPanel : UIPanelBase
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