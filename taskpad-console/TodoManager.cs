class TodoManager
{
    public static List<TaskItem> tasks = new();

    public static void SaveTaskList(List<TaskItem> updatedTask) => tasks = updatedTask;

    public static List<TaskItem> GetTasks() => tasks.Where(task => !task.IsDeleted).ToList();

    public static TaskItem? GetTask(int id) => tasks.Find(task => task.Id == id && !task.IsDeleted);

    public static void AddTask(TaskItem task) => tasks.Add(task);

    public static void UpdateTask(TaskItem task)
    {
        int index = tasks.FindIndex(t => t.Id == task.Id && !task.IsDeleted);
        if (index != -1)
        {
            tasks[index] = task;
        }
    }

    public static void DeleteTask(int id)
    {
        // soft delete
        int index = tasks.FindIndex(t => t.Id == id);
        if (index != -1)
        {
            tasks[index].IsDeleted = true;
        }
    }
}
