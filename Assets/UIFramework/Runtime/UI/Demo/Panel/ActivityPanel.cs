using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 活动面板。
/// 当前 Demo 版本：活动标题 + 倒计时 + 进度条 + 任务列表。
/// </summary>
public class ActivityPanel : UIPanelBase
{
    [Header("Buttons")]
    [SerializeField] private Button btnBack;

    [Header("Texts")]
    [SerializeField] private TMP_Text txtTitle;
    [SerializeField] private TMP_Text txtCountdown;
    [SerializeField] private TMP_Text txtProgress;

    [Header("Progress")]
    [SerializeField] private Slider sliderProgress;

    [Header("Task List")]
    [SerializeField] private Transform taskContentRoot;
    [SerializeField] private ActivityTaskItem taskItemPrefab;

    private readonly List<ActivityTaskData> taskDataList = new List<ActivityTaskData>();
    private readonly List<ActivityTaskItem> taskItemList = new List<ActivityTaskItem>();

    private float remainSeconds = 3600f;

    public override void OnInit()
    {
        base.OnInit();

        BindButtons();
        GenerateMockData();
        RefreshView();
    }

    private void Update()
    {
        if (!IsVisible)
        {
            return;
        }

        UpdateCountdown();
    }

    private void BindButtons()
    {
        if (btnBack != null)
        {
            btnBack.onClick.AddListener(OnClickBack);
        }
    }

    private void GenerateMockData()
    {
        taskDataList.Clear();

        taskDataList.Add(new ActivityTaskData(1, "完成 1 次征兵", 1, 1));
        taskDataList.Add(new ActivityTaskData(2, "完成 3 次资源采集", 2, 3));
        taskDataList.Add(new ActivityTaskData(3, "参与 1 次同盟攻城", 0, 1));
        taskDataList.Add(new ActivityTaskData(4, "提升任意武将 5 级", 3, 5));
        taskDataList.Add(new ActivityTaskData(5, "完成 10 次政务", 10, 10));
    }

    private void RefreshView()
    {
        if (txtTitle != null)
        {
            txtTitle.text = "限时发展活动";
        }

        RefreshProgress();
        RefreshTaskList();
        UpdateCountdownText();
    }

    private void RefreshProgress()
    {
        int completedCount = 0;

        foreach (ActivityTaskData taskData in taskDataList)
        {
            if (taskData.isCompleted)
            {
                completedCount++;
            }
        }

        int totalCount = taskDataList.Count;
        float progress = totalCount > 0 ? (float)completedCount / totalCount : 0f;

        if (sliderProgress != null)
        {
            sliderProgress.value = progress;
        }

        if (txtProgress != null)
        {
            txtProgress.text = $"{completedCount}/{totalCount}";
        }
    }

    private void RefreshTaskList()
    {
        ClearTaskItems();

        foreach (ActivityTaskData data in taskDataList)
        {
            ActivityTaskItem item = Instantiate(taskItemPrefab, taskContentRoot);
            item.SetData(data);
            taskItemList.Add(item);
        }
    }

    private void ClearTaskItems()
    {
        foreach (ActivityTaskItem item in taskItemList)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }

        taskItemList.Clear();
    }

    private void UpdateCountdown()
    {
        if (remainSeconds <= 0f)
        {
            remainSeconds = 0f;
            UpdateCountdownText();
            return;
        }

        remainSeconds -= Time.deltaTime;
        UpdateCountdownText();
    }

    private void UpdateCountdownText()
    {
        int totalSeconds = Mathf.CeilToInt(remainSeconds);
        int hours = totalSeconds / 3600;
        int minutes = totalSeconds % 3600 / 60;
        int seconds = totalSeconds % 60;

        if (txtCountdown != null)
        {
            txtCountdown.text = $"剩余时间：{hours:D2}:{minutes:D2}:{seconds:D2}";
        }
    }

    private void OnClickBack()
    {
        UIManager.Instance.Back();
    }

    protected override void OnDispose()
    {
        if (btnBack != null)
        {
            btnBack.onClick.RemoveListener(OnClickBack);
        }

        base.OnDispose();
    }
}