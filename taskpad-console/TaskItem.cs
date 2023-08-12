enum TaskPriority
{
    HIGH = 1,
    MEDIUM,
    LOW
}
class TaskItem
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime DueDate
    { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public TaskItem()
    {

    }

    public TaskItem(long id, string title, string description, bool isCompleted, DateTime dueDate, TaskPriority taskPriority = TaskPriority.MEDIUM)
    {
        Id = id;
        Title = title;
        Description = description;
        IsCompleted = isCompleted;
        DueDate = dueDate;
        Priority = taskPriority;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        IsDeleted = false;
    }
}
