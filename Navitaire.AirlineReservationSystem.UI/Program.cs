using Navitaire.AirlineReservationSystem.UI.Common;

namespace Navitaire.AirlineReservationSystem.UI
{
    public class Program
    {
        public static void Main(string[]? args = null)
        {
            var options = new List<Option>
            {
                new Option("Flight Maintenance", FlightMaintenanceUI.MenuUI),
                new Option("Reservations", FlightReservationUI.MenuUI),
                new Option("Exit", () => Environment.Exit(0)),
            };

            int selectedIndex = 0;
            string consoleTitle = "Navitaire's Airline Reservation System";
            string initialMessage = "Welcome to Navitaire's Airline Reservation System";
            Menu.WriteMenu(options, options[selectedIndex], consoleTitle, initialMessage);
            Menu.ChooseMenuOption(options, selectedIndex, consoleTitle, initialMessage);
        }
    }
}