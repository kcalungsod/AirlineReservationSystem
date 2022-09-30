using Navitaire.AirlineReservationSystem.Core.Models;

namespace Navitaire.AirlineReservationSystem.Core.Common
{
    public class FetchFactory
    {
        public static IList<Flight>? GetFlights(string type) => type switch
        {
            "TESTDB" => TestDB.CurrentFlights,
            "JSONDB" => JsonFlights.GetCurrentFlightsFromJSON(),
            _ => throw new NotImplementedException()
        };

        public static void SetFlights(string type, Flight newFlight)
        {
            switch (type)
            {
                case "TESTDB":
                    TestDB.Addflight(newFlight);
                    break;
                case "JSONDB":
                    JsonFlights.SetNewFlightInJSON(newFlight);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public static IList<Reservation>? GetReservations(string type) => type switch
        {
            "TESTDB" => TestDB.CurrentReservations,
            "JSONDB" => JsonReservations.GetCurrentReservationsFromJSON(),
            _ => throw new NotImplementedException()
        };

        public static void SetReservations(string type, Reservation newReservation)
        {
            switch (type)
            {
                case "TESTDB":
                    TestDB.AddReservation(newReservation);
                    break;
                case "JSONDB":
                    JsonReservations.SetNewReservationInJSON(newReservation);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

    }
}
