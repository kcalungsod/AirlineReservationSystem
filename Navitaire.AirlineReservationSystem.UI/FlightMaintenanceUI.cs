using Navitaire.AirlineReservationSystem.Core.Models;
using Navitaire.AirlineReservationSystem.Core.Services;
using Navitaire.AirlineReservationSystem.UI.Common;
using Navitaire.AirlineReservationSystem.UI.Fields;

namespace Navitaire.AirlineReservationSystem.UI
{
    public class FlightMaintenanceUI
    {
        public static void MenuUI()
        {
            var options = new List<Option>
            {
                new Option("Add a flight", AddFlightUI),
                new Option("Search existing flights", SearchFlightMenuUI),
                new Option("Back", () => Program.Main()),
            };

            int selectedIndex = 0;
            string consoleTitle = "Flight Maintenance Menu";
            string initialMessage = "Flight Maintenance! Add and Search flights!";
            Menu.WriteMenu(options, options[selectedIndex], consoleTitle, initialMessage);
            Menu.ChooseMenuOption(options, selectedIndex, consoleTitle, initialMessage);
        }

        private static void AddFlightUI()
        {
            Console.Clear();
            Console.Title = "Flight Maintenance - Adding Flights";

            Console.WriteLine("When adding flights, make sure to enter a valid input!");
            ConsoleExtension.ConsoleExitHeader();

            var airlineCode = ReturnValidValue.IfInputIsValid(inputField: FlightField.AirlineCode, MenuUI);
            var flightNumber = int.Parse(ReturnValidValue.IfInputIsValid(inputField: FlightField.FlightNumber, MenuUI)!);

            bool validStations;
            string departureStation;
            string arrivalStation;

            do
            {
                departureStation = ReturnValidValue.IfInputIsValid(inputField: FlightField.DepartureStation, MenuUI)!;
                arrivalStation = ReturnValidValue.IfInputIsValid(inputField: FlightField.ArrivalStation, MenuUI)!;
                validStations = ReturnValidValue.AreStationsDifferent(departureStation!, arrivalStation!);
            }
            while (!validStations);


            var initialScheduledDeparture = ReturnValidValue.IfInputIsValid(inputField: FlightField.ScheduledDeparture, MenuUI);
            var initialScheduledArrival = ReturnValidValue.IfInputIsValid(inputField: FlightField.ScheduledArrival, MenuUI);

            var scheduledDeparture = DateTime.Parse(ReturnValidValue.ForTimeParse(initialScheduledDeparture!));
            var scheduledArrival = DateTime.Parse(ReturnValidValue.ForTimeParse(initialScheduledArrival!));

            Flight newFlight = new()
            {
                AirlineCode = airlineCode,
                FlightNumber = flightNumber,
                DepartureStation = departureStation,
                ArrivalStation = arrivalStation,
                ScheduledTimeOfDeparture = scheduledDeparture,
                ScheduledTimeOfArrival = scheduledArrival,
            };

            var serviceResponse = FlightMaintenance.AddFlight(newFlight);

            if (!serviceResponse) 
            {
                Console.WriteLine("ERROR: Flight can't be added.");
            }
            else 
            { 
                Console.WriteLine("RESPONSE: Flight added successfully."); 
            }

            var rerun = Menu.DoYouWantTo("add another flight");
            if (rerun) { AddFlightUI(); }
            else { MenuUI(); }
        }
        private static void SearchFlightMenuUI()
        {
            var options = new List<Option>
            {
                new Option("Search by Flight Number", SearchByFlightNumber),
                new Option("Search by Airline Code", SearchByAirlineCode),
                new Option("Search by Origin and Destination", SearchByMarket),
                new Option("Back", () => MenuUI()),
            };

            int selectedIndex = 0;
            string consoleTitle = "Flight Maintenance - Search Flight Menu";
            string initialMessage = "Search flight through 3 methods!";
            Menu.WriteMenu(options, options[selectedIndex], consoleTitle, initialMessage);
            Menu.ChooseMenuOption(options, selectedIndex, consoleTitle, initialMessage);
        }

        private static void SearchByFlightNumber()
        {
            Console.Clear();
            Console.Title = "Flight Maintenance - Search Flight: Flight Number";
            Console.WriteLine("Please enter a valid flight number!");
            ConsoleExtension.ConsoleExitHeader();

            var flightNumber = int.Parse(ReturnValidValue.IfInputIsValid(inputField: FlightField.FlightNumber, SearchFlightMenuUI)!);

            var searchedFlights = FlightMaintenance.SearchByFlightNumber(flightNumber);

            if (searchedFlights?.Count() > 0)
            {
                DisplayFlights(searchedFlights);
            }
            else
            {
                Console.WriteLine("RESPONSE: There are no flights with this flight number.");
            }

            var rerun = Menu.DoYouWantTo("search another flight using this method");
            if (rerun) { SearchByFlightNumber(); }
            else { SearchFlightMenuUI(); }
        }

        private static void SearchByAirlineCode()
        {
            Console.Clear();
            Console.Title = "Flight Maintenance - Search Flight: Airline Code";
            Console.WriteLine("Please enter a valid airline code!");
            ConsoleExtension.ConsoleExitHeader();

            var airlineCode = ReturnValidValue.IfInputIsValid(inputField: FlightField.AirlineCode, SearchFlightMenuUI);
            var searchedFlights = FlightMaintenance.SearchByAirlineCode(airlineCode!);

            if (searchedFlights?.Count() > 0)
            {
                DisplayFlights(searchedFlights);
            }
            else
            {
                Console.WriteLine("RESPONSE: There are no flights with this airline code.");
            }

            var rerun = Menu.DoYouWantTo("search another flight using this method");
            if (rerun) { SearchByAirlineCode(); }
            else { SearchFlightMenuUI(); }
        }

        private static void SearchByMarket()
        {
            Console.Clear();
            Console.Title = "Flight Maintenance - Search Flight: Origin and Destination";
            Console.WriteLine("Please enter a valid origin and destination station!");
            ConsoleExtension.ConsoleExitHeader();

            bool validStations;
            string departureStation;
            string arrivalStation;

            do
            {
                departureStation = ReturnValidValue.IfInputIsValid(inputField: FlightField.DepartureStation, SearchFlightMenuUI)!;
                arrivalStation = ReturnValidValue.IfInputIsValid(inputField: FlightField.ArrivalStation, SearchFlightMenuUI)!;
                validStations = ReturnValidValue.AreStationsDifferent(departureStation!, arrivalStation!);
            }
            while (!validStations);

            var searchedFlights = FlightMaintenance.SearchByMarket(departureStation!, arrivalStation!);

            if (searchedFlights?.Count() > 0)
            {
                DisplayFlights(searchedFlights);
            }
            else
            {
                Console.WriteLine("RESPONSE: There are no flights with this origin and destination.");
            }

            var rerun = Menu.DoYouWantTo("search another flight using this method");
            if (rerun) { SearchByMarket(); }
            else { SearchFlightMenuUI(); }
        }

        public static void DisplayFlights(IEnumerable<Flight> searchedFlights)
        {
            Console.WriteLine("\n\n");
            Console.Write("#\t");
            Console.Write($"Airline Code \t");
            Console.Write($"Flight Number \t");
            Console.Write($"Departure Station \t");
            Console.Write($"Arrival Station \t");
            Console.Write($"STD \t\t");
            Console.Write($"STA \t\t");

            int count = 0;

            foreach (var flight in searchedFlights)
            {

                Console.WriteLine("\n");
                Console.Write($"[{++count}] \t");
                Console.Write($"{flight.AirlineCode} \t\t");
                Console.Write($"{flight.FlightNumber} \t\t");
                Console.Write($"{flight.DepartureStation} \t\t\t");
                Console.Write($"{flight.ArrivalStation} \t\t\t");
                Console.Write($"{flight.ScheduledTimeOfDeparture!.Value:HH:mm} \t\t");
                Console.Write($"{flight.ScheduledTimeOfArrival!.Value:HH:mm}");
            }
        }
    }
}
