namespace Navitaire.AirlineReservationSystem.Core.Common
{
    public class ServerValidators
    {
        public static bool CommonErrorChecks(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            if (input.HasSpecialChars())
            {
                return false;
            }
            return true;
        }

        public static bool ValidateAirlineCode(string? airlineCode)
        {
            var commonErrorCheck = CommonErrorChecks(input: airlineCode);

            if (!commonErrorCheck)
            {
                return commonErrorCheck;
            }
            if (airlineCode?.Length != 2)
            {
                return false;
            }
            if (int.TryParse(airlineCode, out _))
            {
                return false;
            }
            if (int.TryParse(airlineCode.Substring(1, 1), out _))
            {
                return false;
            }

            return true;
        }

        public static bool ValidateFlightNumber(string? flightNumber)
        {
            var commonErrorCheck = CommonErrorChecks(input: flightNumber);

            if (!commonErrorCheck)
            {
                return commonErrorCheck;
            }
            if (flightNumber?.Length > 4)
            {
                return false;
            }
            if (!int.TryParse(flightNumber, out int parsedFlightNumber))
            {
                return false;
            }
            if (parsedFlightNumber < 1 || parsedFlightNumber > 9999)
            {
                return false;
            }

            return true;
        }

        public static bool ValidateStation(string? station)
        {
            var commonErrorCheck = CommonErrorChecks(input: station);

            if (!commonErrorCheck)
            {
                return commonErrorCheck;
            }
            if (station?.Length != 3)
            {
                return false;
            }
            if (int.TryParse(station.Substring(0, 1), out int _))
            {
                return false;
            }

            return true;
        }

        public static bool ValidateDepartureAndArrivalStation(string? origin, string? destination)
        {
            if (origin?.ToUpper() == destination?.ToUpper())
            {
                return false;
            }

            return true;
        }

        public static bool ValidateScheduledTime(string? time)
        {
            if (string.IsNullOrEmpty(time))
            {
                return false;
            }
            if (!DateTime.TryParse(time, out _))
            {
                return false;
            }
            return true;
        }

        public static bool ValidateFlightDate(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            if (!DateTime.TryParse(input, out DateTime parsedDate))
            {
                return false;
            }
            if (parsedDate.Date < DateTime.Now.Date)
            {
                return false;
            }

            return true;
        }

        public static bool ValidatePassengerCount(string? input, int listOfPassengerCount)
        {
            var commonErrorCheck = CommonErrorChecks(input: input);

            if (!commonErrorCheck)
            {
                return commonErrorCheck;
            }
            if (!int.TryParse(input, out int numberOfPassengers))
            {
                return false;
            }
            if (numberOfPassengers <= 0)
            {
                return false;
            }
            if (numberOfPassengers > 5)
            {
                return false;
            }
            if (numberOfPassengers != listOfPassengerCount)
            {
                return false;
            }

            return true;
        }

        public static bool ValidatePNR(string? input)
        {
            var commonErrorCheck = CommonErrorChecks(input: input);

            if (!commonErrorCheck)
            {
                return commonErrorCheck;
            }
            if (input?.Length != 6)
            {
                return false;
            }
            if (int.TryParse(input.Substring(0, 1), out _))
            {
                return false;
            }

            return true;
        }

        public static bool ValidateName(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            if (input?.Length > 20)
            {
                return false;
            }

            return true;
        }

        public static bool ValidateBirthDate(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            if (!DateTime.TryParse(input, out DateTime parsedDate))
            {
                return false;
            }
            if (parsedDate > DateTime.Now)
            {
                return false;
            }
            if (parsedDate.Year < 1850)
            {
                return false;
            }

            return true;
        }
    }
}
