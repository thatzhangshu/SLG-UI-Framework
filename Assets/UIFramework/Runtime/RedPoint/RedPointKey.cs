/// <summary>
/// 红点 Key 定义。
/// 使用树状 key，避免代码中到处手写字符串。
/// </summary>
public static class RedPointKey
{
    public const string Root = "Root";
    public const string MainHUD = "MainHUD";

    // Mail
    public const string Mail = "Mail";
    public const string MailUnread = "Mail.Unread";
    public const string MailReward = "Mail.Reward";

    // Hero
    public const string Hero = "Hero";
    public const string HeroNewHero = "Hero.NewHero";
    public const string HeroUpgradeable = "Hero.Upgradeable";

    // Chat
    public const string Chat = "Chat";
    public const string ChatUnread = "Chat.Unread";

    // Activity
    public const string Activity = "Activity";
    public const string ActivityLoginReward = "Activity.LoginReward";
    public const string ActivityDailyTask = "Activity.DailyTask";
    public const string ActivityLimitedEvent = "Activity.LimitedEvent";

    // TODO：后续系统预留
    // public const string Bag = "Bag";
    // public const string BagNewItem = "Bag.NewItem";

    // public const string Alliance = "Alliance";
    // public const string AllianceHelp = "Alliance.Help";
    // public const string AllianceGift = "Alliance.Gift";

    // public const string World = "World";
    // public const string WorldCity = "World.City";
    // public const string WorldTroop = "World.Troop";
}