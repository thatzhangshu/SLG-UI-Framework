using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 在脚本资源变更后自动重新生成 IDE 工程文件（.sln / .csproj），
/// 避免在 Cursor / VS Code 中出现“Unity 能编译但 IDE 报红线”的问题。
/// </summary>
public static class ForceGenerateProjectFiles
{
    private const double SyncDelaySeconds = 0.5d;
    private static double nextSyncTime;
    private static bool syncScheduled;

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        EditorApplication.projectChanged += ScheduleSync;
    }

    [MenuItem("Tools/UI Framework/Regenerate IDE Project Files")]
    public static void RegenerateProjectFilesMenu()
    {
        SyncProjectFiles(forceLog: true);
    }

    public static void ScheduleSync()
    {
        nextSyncTime = EditorApplication.timeSinceStartup + SyncDelaySeconds;
        if (syncScheduled)
        {
            return;
        }

        syncScheduled = true;
        EditorApplication.update += WaitAndSync;
    }

    private static void WaitAndSync()
    {
        if (EditorApplication.timeSinceStartup < nextSyncTime)
        {
            return;
        }

        EditorApplication.update -= WaitAndSync;
        syncScheduled = false;
        SyncProjectFiles(forceLog: false);
    }

    private static void SyncProjectFiles(bool forceLog)
    {
        bool synced = TrySyncSolution();
        if (forceLog)
        {
            Debug.Log(synced
                ? "[UIFramework] IDE 工程文件已重新生成。"
                : "[UIFramework] 未能自动重新生成 IDE 工程文件，请在 Unity 中执行 Edit > Preferences > External Tools > Regenerate project files。");
        }
    }

    private static bool TrySyncSolution()
    {
        Type syncVsType = Type.GetType("UnityEditor.SyncVS, UnityEditor");
        MethodInfo syncMethod = syncVsType?.GetMethod(
            "SyncSolution",
            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        if (syncMethod != null)
        {
            syncMethod.Invoke(null, null);
            return true;
        }

        return false;
    }

    public static bool HasScriptChanges(string[] assets)
    {
        if (assets == null)
        {
            return false;
        }

        foreach (string asset in assets)
        {
            if (asset.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}

/// <summary>
/// 监听脚本导入/删除/移动，触发 IDE 工程文件同步。
/// </summary>
public class ForceGenerateProjectFilesPostprocessor : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        if (!ForceGenerateProjectFiles.HasScriptChanges(importedAssets) &&
            !ForceGenerateProjectFiles.HasScriptChanges(deletedAssets) &&
            !ForceGenerateProjectFiles.HasScriptChanges(movedAssets) &&
            !ForceGenerateProjectFiles.HasScriptChanges(movedFromAssetPaths))
        {
            return;
        }

        ForceGenerateProjectFiles.ScheduleSync();
    }
}
