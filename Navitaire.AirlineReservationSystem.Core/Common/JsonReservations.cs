using Navitaire.AirlineReservationSystem.Core.Database;
using Navitaire.AirlineReservationSystem.Core.Models;
using Newtonsoft.Json;

namespace Navitaire.AirlineReservationSystem.Core.Common
{
    public class JsonReservations
    {
        public static IList<Reservation>? GetCurrentReservationsFromJSON()
        {
            ReservationList? currentReservations;

            try
            {
                using (StreamReader reader = new(FilePaths.Reservations))
                {
                    string json = reader.ReadToEnd();
                    currentReservations = JsonConvert.DeserializeObject<ReservationList>(json);
                }

                if (currentReservations?.ReservationDB != null)
                {
                    var (hasDuplicates, noDuplicateReservations) = CheckForDuplicates(currentReservations.ReservationDB);

                    if (hasDuplicates && noDuplicateReservations != null)
                    {
                        currentReservations.ReservationDB = noDuplicateReservations;
                    }

                    var (allValid, invalidReservations) = ValidateJSONReservations(currentReservations.ReservationDB);
                    if (!allValid && invalidReservations != null)
                    {
                        var validatedReservations = RemoveInvalidJSONReservations(currentReservations.ReservationDB.ToList(), invalidReservations);
                        currentReservations.ReservationDB = validatedReservations;
                    }
                }

                return currentReservations?.ReservationDB;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return null;
            }
        }

        public static void SetNewReservationInJSON(Reservation newReservation)
        {
            List<Reservation>? currentReservations = GetCurrentReservationsFromJSON()?.ToList();

            currentReservations?.Add(newReservation);
            ReservationList reservationList = new()
            {
                ReservationDB = currentReservations?.ToArray()
            };


            try
            {
                var jsonData = JsonConvert.SerializeObject(reservationList);
                File.WriteAllText(FilePaths.Reservations, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }

        }
        private static (bool hasDuplicates, Reservation[]? noDuplicateReservations) CheckForDuplicates(Reservation[] reservationDB)
        {
            var noDuplicateReservations = reservationDB.GroupBy(reservation => reservation.PNRNumber)
                                                       .Select(reservation => reservation.First());
            
            if(noDuplicateReservations.Count() == reservationDB.Length)
            {
                return (hasDuplicates: false, null);
            }
            
            ReservationList reservationList = new()
            {
                ReservationDB = noDuplicateReservations.ToArray()
            };

            SetValidatedReservations(reservationList);

            return (hasDuplicates: true, noDuplicateReservations.ToArray());
        }

        private static (bool allValid, List<Reservation>? invalidReservations) ValidateJSONReservations(Reservation[] reservationDB)
        {
            List<bool> validValues = new();
            List<Reservation> invalidReservations = new();

            foreach (Reservation reservation in reservationDB)
            {
                var airlineCode = ServerValidators.ValidateAirlineCode(reservation.FlightDetails?.AirlineCode);
                var flightNumber = ServerValidators.ValidateFlightNumber(reservation.FlightDetails?.FlightNumber.ToString());
                var departureStation = ServerValidators.ValidateStation(reservation.FlightDetails?.DepartureStation);
                var arrivalStation = ServerValidators.ValidateStation(reservation.FlightDetails?.ArrivalStation);
                var stationValidity = ServerValidators.ValidateDepartureAndArrivalStation(reservation.FlightDetails?.DepartureStation, reservation.FlightDetails?.ArrivalStation);
                var scheduledDeparture = ServerValidators.ValidateScheduledTime(reservation.FlightDetails?.ScheduledTimeOfDeparture.ToString());
                var scheduledArrival = ServerValidators.ValidateScheduledTime(reservation.FlightDetails?.ScheduledTimeOfDeparture.ToString());
                var pnrNumber = ServerValidators.ValidatePNR(reservation.PNRNumber);
                var flightDate = ServerValidators.ValidateFlightDate(reservation.FlightDate.ToString());
                var passengerCount = ServerValidators.ValidatePassengerCount(reservation.NumberOfPassengers.ToString(), reservation.ListOfPassengers!.Count());

                List<bool> validPassengers = new();
                foreach (Passenger passenger in reservation.ListOfPassengers!)
                {
                    var firstName = ServerValidators.ValidateName(passenger.FirstName);
                    var lastName = ServerValidators.ValidateName(passenger.LastName);
                    var birthDate = ServerValidators.ValidateBirthDate(passenger.BirthDate.ToString());

                    if (!firstName || !lastName || !birthDate)
                    {
                        validPassengers.Add(false);
                    }
                    else { validPassengers.Add(true); }

                }

                bool valid;
                if (!airlineCode || !flightNumber || !departureStation || !arrivalStation || !stationValidity || !scheduledDeparture 
                    || !scheduledArrival || !pnrNumber || !flightDate || !passengerCount || validPassengers.Contains(false))
                {
                    valid = false;
                    validValues.Add(valid);
                    invalidReservations.Add(reservation);
                }
                else
                {
                    valid = true;
                    validValues.Add(valid);
                }
            }

            if (validValues.Contains(false))
            {
                return (allValid: false, invalidReservations);
            }
            else
            {
                return (allValid: true, null);
            }
        }

        private static Reservation[] RemoveInvalidJSONReservations(List<Reservation> reservations, List<Reservation> invalidReservations)
        {
            foreach(var invalidReservation in invalidReservations)
            {
                reservations.Remove(invalidReservation);
            }

            ReservationList reservationList = new()
            {
                ReservationDB = reservations.ToArray()
            };

            SetValidatedReservations(reservationList);

            return reservationList.ReservationDB;
        }

        private static void SetValidatedReservations(ReservationList reservationList)
            {
                try
                {
                    var jsonData = JsonConvert.SerializeObject(reservationList);
                    File.WriteAllText(FilePaths.Reservations, jsonData);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.Message}");
                }
            }

        
    }
}
