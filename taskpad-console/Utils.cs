
namespace Utilities
{
    public class Utils
    {
        public static string GetAppTitle()
        {
            return @"
████████  █████  ███████ ██   ██ ██████   █████  ██████ 
   ██    ██   ██ ██      ██  ██  ██   ██ ██   ██ ██   ██
   ██    ███████ ███████ █████   ██████  ███████ ██   ██
   ██    ██   ██      ██ ██  ██  ██      ██   ██ ██   ██
   ██    ██   ██ ███████ ██   ██ ██      ██   ██ ██████  ";
        }

        public static string[] GetMenuOptions()
        {
            return new string[] { "Get All Tasks", "View a Task", "Add Task", "Update Task", "Delete Task", "Load Tasks From File", "Save Tasks To File", "Get Specific Tasks", "Clear Console", "Exit" };
        }

        public static void CenterConsoleText(string text, bool endWithNewLine = true, bool isSuccess = false)
        {
            if (string.IsNullOrEmpty(text)) return;

            string[] lines = text.Split('\n');
            int maxWidth = lines.Max(line => line.Length);

            foreach (string line in lines)
            {
                int leftMargin = (Console.WindowWidth - maxWidth) / 2;
                Console.SetCursorPosition(leftMargin, Console.CursorTop);
                if (isSuccess)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                if (endWithNewLine)
                {
                    Console.WriteLine(line);
                }

                else
                {
                    Console.Write(line);
                }
            }
            Console.ResetColor();
        }

        public static void ConsoleErrorMsg(Exception ex)
        {
            Console.SetCursorPosition((Console.WindowWidth - ex.Message.Length) / 2, Console.CursorTop);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error - Message: {ex.Message}");
            Console.ResetColor();
        }

        public static string TakeInput(string inputText, int minimumLength = 0)
        {
            ConsoleKey keyPressed;
            string input = "";
            Utils.CenterConsoleText($"Enter {inputText}: ", false);
            do
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.Escape)
                {
                    return null;
                }

                if (keyPressed == ConsoleKey.Enter)
                {
                    if (input.Length < minimumLength)
                    {
                        Utils.ConsoleErrorMsg(new Exception($"Enter at least {minimumLength} letters"));
                        input = "";
                        Utils.CenterConsoleText($"Enter {inputText}: ", false);
                        continue;
                    }
                    break;
                }

                if (keyPressed == ConsoleKey.Backspace)
                {
                    if (input.Length < 1) continue;
                    (int left, int top) = Console.GetCursorPosition();
                    Console.SetCursorPosition(left - input.Length, top);
                    input = input.Remove(input.Length - 1);
                    Console.Write(input + " ");
                    (left, top) = Console.GetCursorPosition();
                    Console.SetCursorPosition(left - 1, top);
                    continue;
                }

                char pressed = Convert.ToChar(keyPressed);

                pressed = Console.CapsLock ^ keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift) ? Char.ToUpper(pressed) : Char.ToLower(pressed);

                if (keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift) && keyPressed >= ConsoleKey.D0 && keyPressed <= ConsoleKey.D9)
                {
                    char[] shiftNumChars = new char[] { ')', '!', '@', '#', '$', '%', '^', '&', '*', '(' };
                    pressed = shiftNumChars[(int)keyPressed - 48];
                }

                input += pressed;
                Console.Write(pressed);
            } while (true);

            return input;
        }
    }
}
