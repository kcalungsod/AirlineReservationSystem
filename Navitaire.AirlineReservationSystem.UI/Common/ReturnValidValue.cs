namespace Navitaire.AirlineReservationSystem.UI.Common
{
    public class ReturnValidValue
    {
        public static string? IfInputIsValid(string inputField, Action menuMethod)
        {
            bool isValid = false;
            while (!isValid)
            {
                Console.Write($"{inputField}: ");

                string? response = Console.ReadLine()?.Trim();
            
                if (response?.ToUpper() == "X") { menuMethod.Invoke(); }

                var validationResult = InputValidators.Validate(inputField, response);
                isValid = validationResult.isValid;
                if (!isValid)
                {
                    ConsoleExtension.ClearField();
                    Console.WriteLine($"ERROR: {validationResult.errorMessage}. Press any key...");
                    Console.ReadKey();
                    ConsoleExtension.ClearMessage();
                }
                else
                {
                    return response?.ToUpper();
                }
            }
            return null;
        }

        public static string? IfInputIsValid(string inputField, int? maxCount, Action menuMethod)
        {
            bool isValid = false;
            while (!isValid)
            {
                Console.Write($"{inputField}: ");

                string? response = Console.ReadLine()?.Trim();

                if (response?.ToUpper() == "X") { menuMethod.Invoke(); }

                var validationResult = InputValidators.ValidateCommonNumberInput(response, maxCount);

                isValid = validationResult.isValid;
                if (!isValid)
                {
                    ConsoleExtension.ClearMessage();
                    Console.WriteLine($"ERROR: {validationResult.errorMessage}. Press any key...");
                    Console.ReadKey();
                    ConsoleExtension.ClearMessage();
                }
                else
                {
                    return response;
                }
            }
            return null;
        }

        public static bool AreStationsDifferent(string departureStation, string arrivalStation)
        {
            var (isValid, errorMessage) = InputValidators.ValidateDepartureAndArrivalStation(origin: departureStation, destination: arrivalStation);
            
            if (isValid)
            {
                return true;
            }

            ConsoleExtension.ClearConsecutiveFields();
            Console.WriteLine($"ERROR: {errorMessage}. Press any key...");
            Console.ReadKey();
            ConsoleExtension.ClearMessageForTwoConsecutiveFields();
            ConsoleExtension.ClearField();

            return false;
        }

        public static string ForTimeParse(string input)
        {
            var hours = input.Substring(0, 2);
            var minutes = input.Substring(2, 2);

            return $"{hours}:{minutes}";
        }
    }
}
