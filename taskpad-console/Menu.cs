using Utilities;
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

        if (menuTitle.Length > 0)
        {
            string[] titleParts = menuTitle.Split("\n");
            foreach (string titlePart in titleParts)
            {
                Utils.CenterConsoleText(titlePart);
            }
        }


        for (int i = 0; i < menuOptions.Length; i++)
        {

            string menuPrefix;
            if (i == selectedIndex)
            {
                menuPrefix = "=>";
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
            }
            else
            {
                menuPrefix = "  ";
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }

            Utils.CenterConsoleText($" {menuPrefix} {menuOptions[i]}   ");
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
}
