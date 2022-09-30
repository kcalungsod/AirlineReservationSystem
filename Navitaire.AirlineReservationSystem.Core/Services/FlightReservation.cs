using Navitaire.AirlineReservationSystem.Core.Common;
using Navitaire.AirlineReservationSystem.Core.Models;

namespace Navitaire.AirlineReservationSystem.Core.Services
{
    public class FlightReservation
    {
        public static string DatabaseType { get; set; } = "JSONDB";

        public static (bool isSaved, Reservation? finalBooking) CreateBooking(Reservation newReservation)
        {
            bool pnrIsUnique;
            bool pnrIsValid;

            do
            {
                newReservation.PNRNumber = GeneratePNRNumber();
                pnrIsUnique = CheckIfPNRNumberExists(newReservation.PNRNumber);
                pnrIsValid = ServerValidators.ValidatePNR(newReservation.PNRNumber);
            }
            while (!pnrIsUnique || !pnrIsValid);

            var previousReservationCount = FetchFactory.GetReservations(DatabaseType)?.Count;
            FetchFactory.SetReservations(DatabaseType, newReservation);

            var newReservationCount = FetchFactory.GetReservations(DatabaseType)?.Count;

            if (newReservationCount <= previousReservationCount) 
            {
                return (isSaved: false, finalBooking: null);
            }

            return (isSaved: true, finalBooking: newReservation);
        }

        public static string GeneratePNRNumber()
        {
            char[] randomCharacters = "ABCDEFGHIJKLMOPQRSTUVWXYZ".ToCharArray();
            Random random = new Random();
            int alphabet = random.Next(0, 25);
            int num = random.Next(0, 5);
            var _PNRNumber = randomCharacters[alphabet] + Guid.NewGuid().ToString().Substring(num, 5).ToUpper();

            return _PNRNumber;
        }

        public static bool CheckIfPNRNumberExists(string _PNRNumber)
        {
            var currentReservations = FetchFactory.GetReservations(DatabaseType);

            if (currentReservations?.Count() < 1) { return false; }

            var result = currentReservations!.Where(booking => booking.PNRNumber == _PNRNumber);
            
            if(result.Any())
            {
                return true;
            };
            return false;
        }

        public static IEnumerable<Reservation>? GetAllReservations()
        {
            return FetchFactory.GetReservations(DatabaseType);
        }

        public static Reservation? GetReservation(string PNRNumber)
        {
            var currentReservations = FetchFactory.GetReservations(DatabaseType);

            return currentReservations?.SingleOrDefault((booking) => booking.PNRNumber == PNRNumber);
        }

    }
}
