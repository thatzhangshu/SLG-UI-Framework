/// <summary>
/// 层级类型 实质上是优先级 对应Canvas的Sorting Layer
/// 直观来说就是都出现时候谁在谁上
/// </summary>
public enum UILayer
{
    Background,
    HUD,
    Panel,
    Popup,
    Toast,
    System,
    World
}
