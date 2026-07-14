/// <summary>
/// 游戏事件 Key 定义。
/// 统一管理事件名，避免代码中到处手写字符串。
/// </summary>
public static class GameEvent
{
    // Mail
    public const string MailListChanged = "Mail.ListChanged";
    public const string MailRead = "Mail.Read";
    public const string MailDeleted = "Mail.Deleted";
    public const string MailUnreadCountChanged = "Mail.UnreadCountChanged";

    // Hero
    public const string HeroListChanged = "Hero.ListChanged";
    public const string HeroAdded = "Hero.Added";
    public const string HeroUpgradeableChanged = "Hero.UpgradeableChanged";

    // Activity
    public const string ActivityChanged = "Activity.Changed";
    public const string ActivityRewardChanged = "Activity.RewardChanged";
    public const string ActivityTaskCompleted = "Activity.TaskCompleted";

    // UI
    public const string UIOpened = "UI.Opened";
    public const string UIClosed = "UI.Closed";

    // RedPoint
    public const string RedPointChanged = "RedPoint.Changed";

    // TODO：后续可继续扩展
    // public const string BagChanged = "Bag.Changed";
    // public const string AllianceChanged = "Alliance.Changed";
    // public const string WorldUIChanged = "WorldUI.Changed";
}