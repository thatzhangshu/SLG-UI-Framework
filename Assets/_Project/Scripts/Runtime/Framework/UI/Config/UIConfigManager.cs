using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI 配置管理器。
/// 
/// MVP 阶段先使用代码注册 UI 配置，
/// 后续可以替换为 TSV / JSON / ScriptableObject / 导表系统。
/// </summary>
public static class UIConfigManager
{
    private static readonly Dictionary<string, UIConfigItem> configDict =
        new Dictionary<string, UIConfigItem>();

    public static bool IsInitialized { get; private set; }

    public static void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }

        configDict.Clear();

        RegisterDefaultConfigs();

        IsInitialized = true;

        Debug.Log($"[UIConfigManager] Initialized. Count = {configDict.Count}");
    }

    private static void RegisterDefaultConfigs()
    {
        Register(new UIConfigItem(UIName.MailPanel, "Prefabs/Panel/MailPanel"));
        Register(new UIConfigItem(UIName.MailDetailPopup, "Prefabs/Popup/MailDetailPopup"));

        // TODO：后续逐步接入
        // Register(new UIConfigItem(UIName.MainHUD, "UI/HUD/MainHUD"));
        // Register(new UIConfigItem(UIName.HeroPanel, "UI/Panel/HeroPanel"));
        // Register(new UIConfigItem(UIName.ChatPanel, "UI/Panel/ChatPanel"));
        // Register(new UIConfigItem(UIName.ActivityPanel, "UI/Panel/ActivityPanel"));
        // Register(new UIConfigItem(UIName.ConfirmPopup, "UI/Popup/ConfirmPopup"));
        // Register(new UIConfigItem(UIName.Toast, "UI/Toast/Toast"));
    }

    private static void Register(UIConfigItem config)
    {
        if (config == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(config.Name))
        {
            Debug.LogError("[UIConfigManager] Register failed. Name is empty.");
            return;
        }

        if (string.IsNullOrEmpty(config.PrefabPath))
        {
            Debug.LogError($"[UIConfigManager] Register failed. PrefabPath is empty. UIName = {config.Name}");
            return;
        }

        if (configDict.ContainsKey(config.Name))
        {
            Debug.LogWarning($"[UIConfigManager] Duplicate UI config. UIName = {config.Name}");
        }

        configDict[config.Name] = config;
    }

    public static UIConfigItem GetConfig(string uiName)
    {
        if (!IsInitialized)
        {
            Initialize();
        }

        if (string.IsNullOrEmpty(uiName))
        {
            Debug.LogError("[UIConfigManager] GetConfig failed. uiName is empty.");
            return null;
        }

        if (configDict.TryGetValue(uiName, out UIConfigItem config))
        {
            return config;
        }

        Debug.LogError($"[UIConfigManager] UI config not found. UIName = {uiName}");
        return null;
    }

    public static void DebugPrint()
    {
        Debug.Log("========== UIConfigManager Debug ==========");

        foreach (KeyValuePair<string, UIConfigItem> pair in configDict)
        {
            Debug.Log($"UIName = {pair.Key}, PrefabPath = {pair.Value.PrefabPath}");
        }

        Debug.Log("===========================================");
    }
}