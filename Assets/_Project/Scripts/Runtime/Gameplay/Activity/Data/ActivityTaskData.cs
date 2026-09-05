/// <summary>
/// 活动任务数据。
/// </summary>
public class ActivityTaskData
{
    public int taskId;
    public string taskName;
    public int currentProgress;
    public int targetProgress;
    public bool isCompleted;

    public ActivityTaskData(
        int taskId,
        string taskName,
        int currentProgress,
        int targetProgress)
    {
        this.taskId = taskId;
        this.taskName = taskName;
        this.currentProgress = currentProgress;
        this.targetProgress = targetProgress;
        this.isCompleted = currentProgress >= targetProgress;
    }
}