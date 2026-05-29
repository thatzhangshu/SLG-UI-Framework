using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 武将页面。
/// 用于展示和管理玩家拥有的武将。
/// </summary>
public class HeroPanel : UIPanelBase
{
    [SerializeField] private Button btnBack;
    public override void OnInit()
    {
        Debug.Log("HeroPanel OnInit");
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
        Debug.Log("OnClickBack");
        UIManager.Instance.Back();
    }
}
