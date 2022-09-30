using Navitaire.AirlineReservationSystem.Core.Database;
using Navitaire.AirlineReservationSystem.Core.Models;
using Newtonsoft.Json;

namespace Navitaire.AirlineReservationSystem.Core.Common
{
    public static class JsonFlights
    {
        public static IList<Flight>? GetCurrentFlightsFromJSON()
        {
            FlightList? currentFlights;

            try
            {
                using (StreamReader reader = new(FilePaths.Flights))
                {
                    string json = reader.ReadToEnd();
                    currentFlights = JsonConvert.DeserializeObject<FlightList>(json);
                }

                if (currentFlights?.FlightDB != null)
                {
                    var (hasDuplicates, noDuplicateFlights) = CheckForDuplicates(currentFlights.FlightDB);
                    if(hasDuplicates && noDuplicateFlights != null)
                    {
                        currentFlights.FlightDB = noDuplicateFlights;
                    }

                    var (allValid, invalidFlights) = ValidateJSONFlights(currentFlights.FlightDB);
                    if (!allValid && invalidFlights != null)
                    {
                        var validFlights = RemoveInvalidJSONFlights(currentFlights.FlightDB.ToList(), invalidFlights);
                        currentFlights.FlightDB = validFlights;
                    }
                }

                return currentFlights?.FlightDB;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return null;
            }
        }

        public static void SetNewFlightInJSON(Flight newFlight)
        {
            List<Flight>? currentFlights = GetCurrentFlightsFromJSON()?.ToList();
            currentFlights?.Add(newFlight);

            FlightList flightList = new()
            {
                FlightDB = currentFlights?.ToArray()
            };

            try
            {
                var jsonData = JsonConvert.SerializeObject(flightList);
                File.WriteAllText(FilePaths.Flights, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }

        private static (bool hasDuplicates, Flight[]? noDuplicateFlights) CheckForDuplicates(Flight[] flightDB)
        {
            var noDuplicateFlights = flightDB.GroupBy(flight => new { flight.AirlineCode, flight.FlightNumber, flight.ArrivalStation, flight.DepartureStation })
                                             .Select(flight => flight.First());


            if (noDuplicateFlights.Count() == flightDB.Length)
            {
                return (hasDuplicates: false, null);
            }

            FlightList flightList = new()
            {
                FlightDB = noDuplicateFlights.ToArray()
            };

            SetValidatedFlights(flightList);

            return (hasDuplicates: true, noDuplicateFlights.ToArray());
        }

        private static (bool allValid, List<Flight>? invalidFlights) ValidateJSONFlights(Flight[] flightDB)
        {
            List<bool> validValues = new();
            List<Flight> invalidFlights = new();

            foreach (Flight flight in flightDB)
            {
                var airlineCode = ServerValidators.ValidateAirlineCode(flight.AirlineCode);
                var flightNumber = ServerValidators.ValidateFlightNumber(flight.FlightNumber.ToString());
                var departureStation = ServerValidators.ValidateStation(flight.DepartureStation);
                var arrivalStation = ServerValidators.ValidateStation(flight.ArrivalStation);
                var stationValidity = ServerValidators.ValidateDepartureAndArrivalStation(flight.DepartureStation, flight.ArrivalStation);
                var scheduledDeparture = ServerValidators.ValidateScheduledTime(flight.ScheduledTimeOfDeparture.ToString());
                var scheduledArrival = ServerValidators.ValidateScheduledTime(flight.ScheduledTimeOfDeparture.ToString());

                bool valid;
                if (!airlineCode || !flightNumber || !departureStation || !arrivalStation 
                    || !stationValidity || !scheduledDeparture || !scheduledArrival)
                {
                    valid = false;
                    validValues.Add(valid);
                    invalidFlights.Add(flight);
                }
                else
                {
                    valid = true;
                    validValues.Add(valid);
                }
            }

            if (validValues.Contains(false))
            {
                return (allValid: false, invalidFlights);
            }
            else
            {
                return (allValid: true, null);
            }
        }

        private static Flight[] RemoveInvalidJSONFlights(List<Flight> flights, List<Flight> invalidFlights)
        {
            foreach (var invalidFlight in invalidFlights)
            {
                flights.Remove(invalidFlight);
            }

            FlightList validFlights = new()
            {
                FlightDB = flights.ToArray()
            };

            SetValidatedFlights(validFlights);

            return validFlights.FlightDB;
        }

        private static void SetValidatedFlights(FlightList validFlights)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(validFlights);
                File.WriteAllText(FilePaths.Flights, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }
        
    }
}