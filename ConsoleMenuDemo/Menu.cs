class Menu
{
    private int selectedIndex;
    private readonly string[] menuOptions;
    private readonly string menuTitle;

    public Menu(string[] menuOptions, string menuTitle)
    {
        this.menuOptions = menuOptions;
        this.menuTitle = menuTitle;
        this.selectedIndex = 0;
    }

    public void DisplayOptions()
    {

        CenterConsoleText(menuTitle);
        Console.WriteLine("\n");
        for (int i = 0; i < menuOptions.Length; i++)
        {

            string menuPrefix;
            if (i == selectedIndex)
            {
                menuPrefix = ">";
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
            }
            else
            {
                menuPrefix = " ";
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }

            CenterConsoleText($" {menuPrefix} {menuOptions[i]}  ");
        }
        Console.ResetColor();
    }

    public int Run()
    {
        ConsoleKey keyPressed;

        do
        {
            // clear the console and display the options again

            Console.Clear();
            DisplayOptions();

            // get the pressed key

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            keyPressed = keyInfo.Key;

            // update selectedIndex

            if (keyPressed == ConsoleKey.UpArrow)
            {
                selectedIndex = selectedIndex == 0 ? menuOptions.Length - 1 : selectedIndex - 1;
            }
            else if (keyPressed == ConsoleKey.DownArrow)
            {
                selectedIndex = (selectedIndex + 1) % menuOptions.Length;
            }


        } while (keyPressed != ConsoleKey.Enter);

        return selectedIndex;
    }

    public static void CenterConsoleText(string text)
    {

        string[] lines = text.Split('\n');
        int maxWidth = lines.Max(line => line.Length);

        foreach (string line in lines)
        {
            int leftMargin = (Console.WindowWidth - maxWidth) / 2;
            Console.SetCursorPosition(leftMargin, Console.CursorTop);
            Console.WriteLine(line);
        }
    }

}
