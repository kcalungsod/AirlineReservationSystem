namespace Navitaire.AirlineReservationSystem.UI.Common
{
    public class Menu { 
        public static void WriteMenu(List<Option> options, Option selectedOption, string consoleTitle = "", string initialMessage = "")
        {
            Console.Clear();

            Console.Title = consoleTitle;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{initialMessage}");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Note: Navigate the menu using up and down arrow keys.");
            Console.ResetColor();

            Console.WriteLine("Please select an option:");
            foreach (Option option in options)
            {
                if (option == selectedOption)
                {
                    Console.Write(">");
                }
                else
                {
                    Console.Write(" ");
                }

                Console.WriteLine(option.Name);
            }
        }

        public static void ChooseMenuOption(List<Option> options, int index, string consoleTitle, string initialMessage)
        {
            ConsoleKeyInfo keyinfo;
            while (true)
            {
                keyinfo = Console.ReadKey(true);
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < options.Count)
                    {
                        index++;
                        WriteMenu(options, options[index], consoleTitle, initialMessage);
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteMenu(options, options[index], consoleTitle, initialMessage);
                    }
                }
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    options[index]?.Method?.Invoke();
                    index = 0;
                }
            }
        }

        public static bool DoYouWantTo(string message)
        {
            Console.WriteLine("\n");
            bool isValid = false;
            while (!isValid)
            {
                Console.Write($"Do you want to {message}? [Y/N] \t");
                string? formattedAnswer = Console.ReadLine()?.ToUpper();

                if (formattedAnswer == "Y" || formattedAnswer == "YES")
                {
                    return true;
                }
                else if (formattedAnswer == "N" || formattedAnswer == "NO")
                {
                    return false;
                }
                else
                {
                    ConsoleExtension.ClearMessage();
                    Console.WriteLine($"Error: Please enter a valid input. Press any key...");
                    Console.ReadKey();
                    ConsoleExtension.ClearMessage();
                }
            }
            return false;
        }
    }
}
