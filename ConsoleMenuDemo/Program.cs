using static System.Net.Mime.MediaTypeNames;

class Program
{
    public static void Main(string[] args)
    {
        string[] menuOptions = new string[] { "Get All Tasks", "Add Task", "Update Task", "Delete Task" };
        string menuTitle = @"
████████  █████  ███████ ██   ██ ██████   █████  ██████  
   ██    ██   ██ ██      ██  ██  ██   ██ ██   ██ ██   ██ 
   ██    ███████ ███████ █████   ██████  ███████ ██   ██ 
   ██    ██   ██      ██ ██  ██  ██      ██   ██ ██   ██ 
   ██    ██   ██ ███████ ██   ██ ██      ██   ██ ██████";

        Menu menu = new(menuOptions, menuTitle);


        menu.DisplayOptions();
        menu.Run();
    }
}