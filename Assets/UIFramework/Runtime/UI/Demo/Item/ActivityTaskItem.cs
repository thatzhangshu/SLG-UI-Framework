using TMPro;
using UnityEngine;

/// <summary>
/// 活动任务 Item。
/// </summary>
public class ActivityTaskItem : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TMP_Text txtTaskName;
    [SerializeField] private TMP_Text txtProgress;
    [SerializeField] private TMP_Text txtStatus;

    private ActivityTaskData taskData;

    public void SetData(ActivityTaskData data)
    {
        taskData = data;
        RefreshView();
    }

    private void RefreshView()
    {
        if (taskData == null)
        {
            return;
        }

        if (txtTaskName != null)
        {
            txtTaskName.text = taskData.taskName;
        }

        if (txtProgress != null)
        {
            txtProgress.text = $"{taskData.currentProgress}/{taskData.targetProgress}";
        }

        if (txtStatus != null)
        {
            txtStatus.text = taskData.isCompleted ? "已完成" : "进行中";
        }
    }
}