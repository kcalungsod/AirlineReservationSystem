using Navitaire.AirlineReservationSystem.Core.Common;
using Navitaire.AirlineReservationSystem.Core.Models;
using System.Data;

namespace Navitaire.AirlineReservationSystem.Core.Services
{
    public class FlightMaintenance
    {
        public static string DatabaseType { get; set; } = "JSONDB";

        public static bool AddFlight(Flight validFlight)
        {
            bool flightExists = CheckIfFlightExists(validFlight);

            var previousFlightCount = FetchFactory.GetFlights(DatabaseType)?.Count;

            if (flightExists) { return false; }

            FetchFactory.SetFlights(DatabaseType, validFlight);
            
            var newFlightCount = FetchFactory.GetFlights(DatabaseType)?.Count;

            if(newFlightCount <= previousFlightCount) { return false; }
            
            return true;
        }

        public static bool CheckIfFlightExists(Flight flight)
        {
            var currentFlights = FetchFactory.GetFlights(DatabaseType);

            if (currentFlights?.Count < 1) { return false; }

            return currentFlights!.Any(currentFlight => currentFlight.AirlineCode == flight.AirlineCode &&
                                                        currentFlight.FlightNumber == flight.FlightNumber &&
                                                        currentFlight.DepartureStation == flight.DepartureStation &&
                                                        currentFlight.ArrivalStation == flight.ArrivalStation);
        }

        public static IEnumerable<Flight>? SearchByFlightNumber(int flightNumber)
        {
            var currentFlights = FetchFactory.GetFlights(DatabaseType);

            return currentFlights?.Where((flight) => flight.FlightNumber == flightNumber);
            
        }

        public static IEnumerable<Flight>? SearchByAirlineCode(string airlineCode)
        {
            var currentFlights = FetchFactory.GetFlights(DatabaseType);

            return currentFlights?.Where((flight) => flight.AirlineCode == airlineCode);
        }

        public static IEnumerable<Flight>? SearchByMarket(string departureStation, string arrivalStation)
        {
            var currentFlights = FetchFactory.GetFlights(DatabaseType);

            return currentFlights?.Where((flight) => flight.DepartureStation == departureStation &&
                                                     flight.ArrivalStation == arrivalStation);
        }

        public static IEnumerable<Flight>? SearchByAirlineCodeAndFlightNumber(string airlineCode, int flightNumber)
        {
            var currentFlights = FetchFactory.GetFlights(DatabaseType);

            if (currentFlights?.Count < 1) { return null; }

            return currentFlights?.Where(currentFlight => currentFlight.AirlineCode == airlineCode &&
                                                          currentFlight.FlightNumber == flightNumber);

        }

    }
}
