using Utilities;
using My_Table;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    public static void Main(string[] args)
    {
        List<TaskItem> tasks = new();

        InitializeApp();
        RunApp(ref tasks);
    }

    public static void RunApp(ref List<TaskItem> tasks)
    {
        int exitOptionIndex = Utils.GetMenuOptions().Length;
        int choice = -1;

        do
        {
            DisplayOptions();
            string input = Console.ReadLine();
            if (input == "")
            {
                InitializeApp();
                continue;
            }

            try
            {
                choice = int.Parse(input);
                PerformOperation(choice, ref tasks);
            }
            catch (Exception ex)
            {
                Utils.ConsoleErrorMsg(ex);
            }
        } while (choice != exitOptionIndex);
    }

    public static void DisplayOptions()
    {
        int i = 1;
        string[] menuOptions = Utils.GetMenuOptions();
        Utils.CenterConsoleText("\n\n\n~~~~~ Select a menu to proceed ~~~~~\n");

        foreach (string menu in menuOptions) Utils.CenterConsoleText($"{i++}. {menu,-20}");

        Utils.CenterConsoleText("Input: ", false);
    }

    public static void InitializeApp()
    {
        Console.Clear();
        string appTitle = Utils.GetAppTitle();
        Utils.CenterConsoleText(appTitle);
    }

    public static void PerformOperation(int input, ref List<TaskItem> tasks)
    {
        Console.WriteLine("\n");
        switch (input)
        {
            case 1:
                GetAllTask(tasks);
                break;
            case 2:
                HandleSelectTask(ref tasks, "VIEW");
                break;
            case 3:
                AddNewTask(ref tasks);
                break;
            case 4:
                HandleSelectTask(ref tasks, "UPDATE");
                break;
            case 5:
                HandleSelectTask(ref tasks, "DELETE");
                break;
            case 6:
                tasks = FileHandler.LoadTasks();
                TodoManager.SaveTaskList(new List<TaskItem>(tasks));
                GetAllTask(tasks);
                break;
            case 7:
                FileHandler.SaveTasks(tasks);
                break;
            case 8:
                HandleSpecificTask(tasks);
                break;
            case 9:
                InitializeApp();
                break;
            case 10:
                HandleExit(tasks);
                break;
            default:
                Utils.ConsoleErrorMsg(new InvalidDataException("Enter a valid Choice"));
                break;
        }
    }

    public static void GetAllTask(List<TaskItem> tasks)
    {
        if (tasks.Count == 0)
        {
            Utils.CenterConsoleText("No task found.");
            return;
        }

        MyTable taskTable = new(new string[] { "Title", "Description", "Completed", "Priority", "Due Date" });
        foreach (TaskItem task in tasks)
        {
            taskTable.AddRow(new string[] {
                        task.Title,
                        task.Description,
                        task.IsCompleted.ToString(),
                        task.Priority.ToString(),
                        task.DueDate.ToString()
                    });
        }
        taskTable.DrawTable();
    }

    public static void AddNewTask(ref List<TaskItem> tasks)
    {
        Utils.CenterConsoleText("Press escape any time to skip adding task\n");
        string title = Utils.TakeInput("Task Title", 3);

        if (title == null || title.Length == 0) return;

        Console.WriteLine();

        Utils.CenterConsoleText("\nPress escape any time to skip adding task\n");
        string description = Utils.TakeInput("Task Description", 5);

        if (description == null || description.Length == 0) return;

        Menu priorityOptions = new(new string[] { "High", "Medium", "LOW" }, "Enter Task Priority Level");
        int selectedPriority = priorityOptions.Run();

        Menu dueDateOptions = new(new string[] { "Add Days", "Add Hours", "Add Minutes" }, "Select option to add time to current time for due date");

        int selectedDueDate = dueDateOptions.Run();

        int timeUnits = -1;

        do
        {
            Utils.CenterConsoleText("Enter the unit of time to add: ", false);
            try
            {
                timeUnits = int.Parse(Console.ReadLine());
            }
            catch
            {
                Utils.ConsoleErrorMsg(new Exception("Enter correct unit of time in integer"));
            }
        } while (timeUnits < 1);


        DateTime taskDueDate = DateTime.Now;

        switch (selectedDueDate)
        {
            case 0:
                taskDueDate = taskDueDate.AddDays(timeUnits);
                break;
            case 1:
                taskDueDate = taskDueDate.AddHours(timeUnits);
                break;
            case 3:
                taskDueDate = taskDueDate.AddMinutes(timeUnits);
                break;
        }

        long id = Stopwatch.GetTimestamp();

        TaskItem task = new(id, title, description, false, taskDueDate, (TaskPriority)(selectedPriority + 1));
        tasks.Add(task);

        Utils.CenterConsoleText("Task Added Successfully", true, true);
    }



    public static void HandleSelectTask(ref List<TaskItem> tasks, string mode)
    {
        MyTable myTable = new(new string[] { "Title", "Completed", "Priority", "Due Date" });

        foreach (var task in tasks) myTable.AddRow((new string[] { task.Title, task.IsCompleted.ToString(), task.Priority.ToString(), task.DueDate.ToString() }));

        string[] taskList = myTable.GetFormattedRow().ToArray();

        if (taskList.Length < 2)
        {
            Utils.ConsoleErrorMsg(new Exception($"No task to {mode.ToLower()}"));
            return;
        }

        Utils.CenterConsoleText(taskList[0]);

        string taskHeading = taskList[0];

        taskList = taskList.Skip(1).ToArray();

        Menu updateList = new(taskList, $"Select a task to {mode.ToLower()}\n  {taskHeading}");
        Console.WriteLine();

        int selectedIndex = updateList.Run();

        var viewedTask = tasks[selectedIndex];
        Console.WriteLine();

        switch (mode)
        {
            case "VIEW":
                RenderTaskDetail(viewedTask);
                break;
            case "UPDATE":
                UpdateSelectedTask(viewedTask);
                break;
            case "DELETE":
                tasks.Remove(viewedTask);
                Utils.CenterConsoleText("Task Deleted Successfully", true, true);
                break;
        }
    }

    public static void RenderTaskDetail(TaskItem task)
    {
        Console.WriteLine("\n");
        MyTable taskDetailsTable = new(new string[] { "Id", task.Id.ToString() });

        taskDetailsTable.AddRow(new string[] { "Title", task.Title.ToString() });
        taskDetailsTable.AddRow(new string[] { "Completed", task.IsCompleted.ToString() });
        taskDetailsTable.AddRow(new string[] { "Due Date", task.DueDate.ToString() });
        taskDetailsTable.AddRow(new string[] { "Description", task.Description });
        taskDetailsTable.AddRow(new string[] { "Priority", task.Priority.ToString() });
        taskDetailsTable.AddRow(new string[] { "Created At", task.CreatedAt.ToString() });
        taskDetailsTable.AddRow(new string[] { "Updated At", task.UpdatedAt.ToString() });

        taskDetailsTable.DrawTable();
    }

    public static void UpdateSelectedTask(TaskItem task)
    {
        Utils.CenterConsoleText("Press escape to skip title\n");
        string title = Utils.TakeInput("Task Title", 3);

        Utils.CenterConsoleText("Press escape to skip description\n");
        string description = Utils.TakeInput("Task Description", 5);

        Menu priorityList = new(new string[] { "Do not Update", "High", "Medium", "LOW" }, "Enter Task Priority Level");
        int selectedPriority = priorityList.Run();

        Menu dueDateOptions = new(new string[] { "Do not Update", "Add Days", "Add Hours", "Add Minutes" }, "Select option to add time to current due date");

        int selectedDueDate = dueDateOptions.Run();

        int timeUnits = -1;
        if (selectedDueDate != 0)
        {
            do
            {
                Utils.CenterConsoleText("Enter the unit of time to add: ", false);
                try
                {
                    timeUnits = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Utils.ConsoleErrorMsg(new Exception("Enter correct unit of time in integer"));
                }
            } while (timeUnits < 1);

            switch (selectedDueDate)
            {
                case 1:
                    task.DueDate = task.DueDate.AddDays(timeUnits);
                    break;
                case 2:
                    task.DueDate = task.DueDate.AddHours(timeUnits);
                    break;
                case 3:
                    task.DueDate = task.DueDate.AddMinutes(timeUnits);
                    break;
            }
        }

        Menu isCompletedOptions = new(new string[] { "Do not update", "Yes", "No" }, "Is the task completed");
        int taskCompleted = isCompletedOptions.Run();

        if (taskCompleted != 0) task.IsCompleted = taskCompleted == 1;

        task.Title = title ?? task.Title;
        task.Description = description ?? task.Description;
        task.Priority = selectedPriority == 0 ? task.Priority : (TaskPriority)selectedPriority;

        task.UpdatedAt = DateTime.Now;
        TodoManager.UpdateTask(task);
    }

    public static void HandleExit(List<TaskItem> tasks)
    {
        bool isTaskCountSame = tasks.Count == TodoManager.tasks.Count;

        if (!isTaskCountSame)
        {
            Menu quitOptions = new(new string[] { "Save and Quit", "Do not Save" }, "Tasks are updated but not saved to file. Save tasks");

            int selectedQuitOption = quitOptions.Run();

            if (selectedQuitOption == 0) FileHandler.SaveTasks(tasks);
            return;
        }

        bool areTasksUpdated = false;

        var tempListTasks = new List<TaskItem>(tasks);
        var tempFileTasks = new List<TaskItem>(TodoManager.tasks);

        tempListTasks = tempListTasks.OrderBy(task => task.Id).ToList();
        tempFileTasks = tempFileTasks.OrderBy(task => task.Id).ToList();

        for(int i = 0; i <  tempListTasks.Count; i++)
        {
            if (tasks[i].UpdatedAt != tempFileTasks[i++].UpdatedAt)
            {
                areTasksUpdated = true;
                break;
            }
        }

        if (areTasksUpdated)
        {
            Utils.ConsoleErrorMsg(new Exception("Tasks are updated but not saved to file"));
            Menu quitOptions = new(new string[] { "Save", "No, quit" }, "Tasks are updated but not saved to file. Save tasks");

            int selectedQuitOption = quitOptions.Run();

            if (selectedQuitOption == 0) FileHandler.SaveTasks(tasks);
            return;
        }
    }

    public static void HandleSpecificTask(List<TaskItem> tasks)
    {
        Menu specificTaskOptions = new(new string[] { "Get Sorted Tasks", "Get Filtered Task" }, $"Select an option to view task");
        int selectedOption = specificTaskOptions.Run();

        switch (selectedOption)
        {
            case 0:
                HandleSortOption(tasks);
                break;
            case 1:
                HandleFilterOption(tasks);
                break;
        }
    }

    public static void HandleSortOption(List<TaskItem> tasks)
    {
        Menu sortingOptions = new(new string[] { "By Priority (High to Low)", "By Priority (Low to High)", "By Due Date" }, "");
        int selectedOption = sortingOptions.Run();

        var sortedTasks = new List<TaskItem>(tasks);
        switch (selectedOption)
        {
            case 0:
                sortedTasks.Sort((x, y) => x.Priority - y.Priority);
                break;
            case 1:
                sortedTasks.Sort((x, y) => y.Priority - x.Priority);
                break;
            case 2:
                sortedTasks.Sort((x, y) => DateTime.Compare(x.DueDate, y.DueDate));
                break;
        }
        GetAllTask(sortedTasks);
    }

    public static void HandleFilterOption(List<TaskItem> tasks)
    {
        Menu filterOptions = new(new string[] { "Due Date Remaining", "Due Date Expired", "Priority", "Completed", "Not Completed" }, "");
        int selectedOption = filterOptions.Run();

        var filteredTasks = new List<TaskItem>(tasks);
        switch (selectedOption)
        {
            case 0:
                filteredTasks = filteredTasks.Where(task => task.DueDate > DateTime.Now).ToList();
                break;
            case 1:
                filteredTasks = filteredTasks.Where(task => task.DueDate < DateTime.Now).ToList();
                break;
            case 2:
                filteredTasks = HandlePriorityFilter(filteredTasks);
                break;
            case 3:
                filteredTasks = filteredTasks.Where(task => task.IsCompleted).ToList();
                break;
            case 4:
                filteredTasks = filteredTasks.Where(task => !task.IsCompleted).ToList();
                break;
        }
        GetAllTask(filteredTasks);
    }

    public static List<TaskItem> HandlePriorityFilter(List<TaskItem> tasks)
    {
        Menu priorityFilter = new(new string[] { "High", "Medium", "Low" }, "Choose to filter");
        int selectedOption = priorityFilter.Run();

        switch (selectedOption)
        {
            case 0:
                tasks = tasks.Where(task => task.Priority == TaskPriority.HIGH).ToList();
                break;
            case 1:
                tasks = tasks.Where(task => task.Priority == TaskPriority.MEDIUM).ToList();
                break;
            case 2:
                tasks = tasks.Where(task => task.Priority == TaskPriority.LOW).ToList();
                break;
        }
        return tasks;
    }
}
