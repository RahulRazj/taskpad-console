using System.Text.Json;
using Utilities;

class FileHandler
{
    const string taskJsonFilePath = @"D:\Programs\Kongsberg-training\C#\console-module\taskpad-console\taskpad-console\tasks.json";
    public static void SaveTasks(List<TaskItem> tasks)
    {
        try
        {
            var tasksJson = JsonSerializer.Serialize(tasks);
            File.WriteAllText(taskJsonFilePath, tasksJson);
            Utils.CenterConsoleText("Files Saved Successfully", false, true);
        }
        catch
        {
            throw;
        }
    }

    public static List<TaskItem>? LoadTasks()
    {
        try
        {
            if (!File.Exists(taskJsonFilePath)) throw new FileNotFoundException("Tasks file not found");

            string tasksJson = File.ReadAllText(taskJsonFilePath);
            List<TaskItem> tasks = new();

            if (!string.IsNullOrEmpty(tasksJson)) tasks = JsonSerializer.Deserialize<List<TaskItem>>(tasksJson);

            return tasks;
        }
        catch
        {
            throw;
        }
    }
}
