using Data;
using Enums;
using Interfaces.ReadOnly;
using Managers;

public class Task : IReadOnlyTask
{
    public DialogService dialog;

    public TaskData taskData;

    public string startDialogIds;
    public string processDialogIds;
    public string endDialogIds;
    
    public TaskStatuses taskStatus;

    public TaskBase()
    {
        dialog = new DialogService();
        taskStatus = TaskStatuses.NotStart;
        Initialize();
    }

    public virtual void StartTask()
    {
        taskStatus = TaskStatuses.Process;
    }

    public virtual void EndTask()
    {
        taskStatus = TaskStatuses.Done;
    }
    
    protected virtual void Initialize() { }

    public string Title => taskData.title;
    public string Description => taskData.description;
    public TaskStatuses Status => taskStatus;
}
