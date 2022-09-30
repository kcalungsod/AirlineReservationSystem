using Navitaire.AirlineReservationSystem.Core.Common;
using Navitaire.AirlineReservationSystem.UI.Fields;

namespace Navitaire.AirlineReservationSystem.UI.Common
{
    public class InputValidators
    {
        public static (bool isValid, string? errorMessage) Validate(string? inputValue, string? input) => inputValue switch
        {
            FlightField.AirlineCode => ValidateAirlineCode(input),
            FlightField.FlightNumber => ValidateFlightNumber(input),
            FlightField.DepartureStation => ValidateStation(input, stationType: FlightField.DepartureStation),
            FlightField.ArrivalStation => ValidateStation(input, stationType: FlightField.ArrivalStation),
            FlightField.ScheduledDeparture => ValidateScheduledTime(input, category: FlightField.ScheduledDeparture),
            FlightField.ScheduledArrival => ValidateScheduledTime(input, category: FlightField.ScheduledArrival),
            ReservationField.FlightDate => ValidateFlightDate(input),
            ReservationField.NumberOfPassengers => ValidatePassengerCount(input),
            ReservationField.PNRNumber => ValidatePNR(input),
            PassengerField.FirstName => ValidateName(input, field: PassengerField.FirstName),
            PassengerField.LastName => ValidateName(input, field: PassengerField.LastName),
            PassengerField.BirthDate => ValidateBirthDate(input),
            _ => throw new NotImplementedException(),
        };

        public static (bool isValid, string? errorMessage) ValidateCommonNumberInput(string? input, int? maxCount)
        {
            var commonErrorCheck = CommonErrorChecks(inputField: "The answer to this field", input: input);

            if (!commonErrorCheck.isValid)
            {
                return commonErrorCheck;
            }
            if (!int.TryParse(input, out int parsedInput))
            {
                return (isValid: false, errorMessage: $"The answer to this field can't be a letter");
            }
            if (parsedInput > maxCount)
            {
                return (isValid: false, errorMessage: $"The answer to this field should be within the range of options displayed");
            }
            if (parsedInput <= 0)
            {
                return (isValid: false, errorMessage: $"The answer to this field shouldn't be zero or a negative number");
            }

            return (isValid: true, errorMessage: "");
        }

        public static (bool isValid, string? errorMessage) CommonErrorChecks(string inputField, string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return (isValid: false, errorMessage: $"{inputField} can't be empty");
            }
            if (StringExtension.HasSpecialChars(input))
            {
                return (isValid: false, errorMessage: $"{inputField} shouldn't have special characters or white spaces");
            }
            return (isValid: true, errorMessage: "");
        }

        public static (bool isValid, string? errorMessage) ValidateAirlineCode(string? airlineCode)
        {
            var commonErrorCheck = CommonErrorChecks(inputField: FlightField.AirlineCode, input: airlineCode);

            if (!commonErrorCheck.isValid)
            {
                return commonErrorCheck;
            }
            if (airlineCode?.Length != 2)
            {
                return (isValid: false, errorMessage: $"{FlightField.AirlineCode} should only have 2 characters");
            }
            if (int.TryParse(airlineCode, out _))
            {
                return (isValid: false, errorMessage: $"{FlightField.AirlineCode} characters can't be both numbers");
            }
            if(int.TryParse(airlineCode.Substring(1,1), out _))
            {
                return (isValid: false, errorMessage: $"The 2nd character of an {FlightField.AirlineCode} can't be a digit");
            }

            return (isValid: true, errorMessage: "");
        }

        public static (bool isValid, string? errorMessage) ValidateFlightNumber(string? flightNumber)
        {
            var commonErrorCheck = CommonErrorChecks(inputField: FlightField.FlightNumber, input: flightNumber);

            if (!commonErrorCheck.isValid)
            {
                return commonErrorCheck;
            }
            if (flightNumber?.Length > 4)
            {
                return (isValid: false, errorMessage: $"{FlightField.FlightNumber} maximum valid input is 9999");
            }
            if (!int.TryParse(flightNumber, out int parsedFlightNumber))
            {
                return (isValid: false, errorMessage: $"{FlightField.FlightNumber} should be numeric");
            }
            if (parsedFlightNumber < 1 || parsedFlightNumber > 9999)
            {
                return (isValid: false, errorMessage: $"{FlightField.FlightNumber} should only be from 1-9999");
            }

            return (isValid: true, errorMessage: "");
        }

        public static (bool isValid, string? errorMessage) ValidateStation(string? station, string stationType)
        {
            var commonErrorCheck = CommonErrorChecks(inputField: stationType, input: station);

            if (!commonErrorCheck.isValid)
            {
                return commonErrorCheck;
            }
            if (station?.Length != 3)
            {
                return (isValid: false, errorMessage: $"{stationType} should only have 3 characters");
            }
            if (int.TryParse(station.Substring(0, 1), out int _))
            {
                return (isValid: false, errorMessage: $"{stationType} should start with a letter");
            }

            return (isValid: true, errorMessage: "");
        }

        public static (bool isValid, string? errorMessage) ValidateDepartureAndArrivalStation(string origin, string destination)
        {
            if (origin.ToUpper() == destination.ToUpper())
            {
                return (isValid: false, errorMessage: $"{FlightField.DepartureStation} and {FlightField.ArrivalStation} shouldn't be the same");
            }

            return (isValid: true, errorMessage: "");
        }

        public static (bool isValid, string? errorMessage) ValidateScheduledTime(string? time, string category)
        {
            var commonErrorCheck = CommonErrorChecks(inputField: category, input: time);

            if (!commonErrorCheck.isValid)
            {
                return commonErrorCheck;
            }
            if (time?.Length != 4)
            {
                return (isValid: false, errorMessage: $"{category} isn't in the correct 24-hour Format.");
            }
            if (!int.TryParse(time?.Substring(0, 2), out int hours)) 
            {
                return (isValid: false, errorMessage: $"{category} isn't in the correct 24-hour Format");
            }
            if (!int.TryParse(time?.Substring(2, 2), out int minutes)) 
            {
                return (isValid: false, errorMessage: $"{category} isn't in the correct 24-hour Format");
            }
            if ((hours > 23 || hours < 0) || (minutes > 59 || minutes < 0)){

                return (isValid: false, errorMessage: $"{category} isn't in the correct 24-hour Format");
            }

            return (isValid: true, errorMessage: "");
        }

        public static (bool isValid, string? errorMessage) ValidateFlightDate(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return (isValid: false, errorMessage: $"{ReservationField.FlightDate} should not be empty");
            }
            if (!DateTime.TryParse(input, out DateTime parsedDate))
            {
                return (isValid: false, errorMessage: $"{ReservationField.FlightDate} is not a valid date");
            }
            if (parsedDate.Date < DateTime.Now.Date)
            {
                return (isValid: false, errorMessage: $"{ReservationField.FlightDate} should not be past dated");
            }

            return (isValid: true, errorMessage: "");
        }

        public static (bool isValid, string? errorMessage) ValidatePassengerCount(string? input)
        {
            var commonErrorCheck = CommonErrorChecks(inputField: ReservationField.NumberOfPassengers, input: input);

            if (!commonErrorCheck.isValid)
            {
                return commonErrorCheck;
            }
            if (!int.TryParse(input, out int numberOfPassengers))
            {
                return (isValid: false, errorMessage: $"{ReservationField.NumberOfPassengers} is not a valid input");
            }
            if (numberOfPassengers <= 0)
            {
                return (isValid: false, errorMessage: $"{ReservationField.NumberOfPassengers} should be greater than 0");
            }
            if (numberOfPassengers > 5)
            {
                return (isValid: false, 
                        errorMessage: $"{ReservationField.NumberOfPassengers} can only handle a maximum of 5 passengers per booking");
            }

            return (isValid: true, errorMessage: "");
        }

        public static (bool isValid, string? errorMessage) ValidatePNR(string? input)
        {
            var commonErrorCheck = CommonErrorChecks(inputField: ReservationField.PNRNumber, input: input);

            if (!commonErrorCheck.isValid)
            {
                return commonErrorCheck;
            }
            if (input?.Length != 6)
            {
                return (isValid: false, errorMessage: $"{ReservationField.PNRNumber} should have 6 characters");
            }
            if (int.TryParse(input.Substring(0, 1), out _))
            {
                return (isValid: false, errorMessage: $"{ReservationField.PNRNumber} should start with a letter");
            }

            return (isValid: true, errorMessage: "");
        }

        public static (bool isValid, string? errorMessage) ValidateName(string? input, string field)
        {
            if (string.IsNullOrEmpty(input))
            {
                return (isValid: false, errorMessage: $"{field} can't be empty");
            }
            if (input?.Length > 20)
            {
                return (isValid: false, errorMessage: $"{field} only accepts a maximum of 20 characters");
            }

            return (isValid: true, errorMessage: "");
        }

        public static (bool isValid, string? errorMessage) ValidateBirthDate(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return (isValid: false, errorMessage: $"{PassengerField.BirthDate} should not be empty");
            }
            if (!DateTime.TryParse(input, out DateTime parsedDate))
            {
                return (isValid: false, errorMessage: $"{PassengerField.BirthDate} is not a valid date");
            }
            if (parsedDate > DateTime.Now)
            {
                return (isValid: false, errorMessage: $"{PassengerField.BirthDate} should not be future dated");
            }
            if (parsedDate.Year < 1850)
            {
                return (isValid: false, errorMessage: $"Oldest year allowed for {PassengerField.BirthDate} is limited to 1850");
            }

            return (isValid: true, errorMessage: "");
        }
    }
}
