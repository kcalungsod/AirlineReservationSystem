using Navitaire.AirlineReservationSystem.Core.Models;
using Navitaire.AirlineReservationSystem.Core.Services;
using Navitaire.AirlineReservationSystem.UI.Common;
using Navitaire.AirlineReservationSystem.UI.Fields;

namespace Navitaire.AirlineReservationSystem.UI
{
    public class FlightReservationUI
    {
        public static void MenuUI()
        {
            var options = new List<Option>
            {
                new Option("Create a reservation", CreateReservationUI),
                new Option("List all reservations", ListAllReservationUI),
                new Option("Search a booking", SearchReservationUI),
                new Option("Back", () => Program.Main()),
            };

            int selectedIndex = 0;
            string consoleTitle = "Flight Reservation Menu";
            string initialMessage = "Flight Reservation! Reserve and Search bookings!";
            Menu.WriteMenu(options, options[selectedIndex], consoleTitle, initialMessage);
            Menu.ChooseMenuOption(options, selectedIndex, consoleTitle, initialMessage);
        }

        private static void CreateReservationUI()
        {
            Console.Clear();
            Console.Title = "Reservations - Create a booking";

            Console.WriteLine("When creating bookings, make sure to enter valid inputs!");
            ConsoleExtension.ConsoleExitHeader();

            Reservation newReservation = new();
  
            var (flightIsAvailable, flightSelected) = SearchFlightAvailabilityUI();

            while (!flightIsAvailable)
            {
                (flightIsAvailable, flightSelected) = SearchFlightAvailabilityUI();
            }
            
            newReservation.FlightDetails = flightSelected;

            var (flightDate, numberOfPassengers, passengerList) = AskReservationDetails(flightSelected);
            newReservation.FlightDate = flightDate;
            newReservation.NumberOfPassengers = numberOfPassengers;
            newReservation.ListOfPassengers = passengerList;

            ReservationSummary(newReservation);
            bool willBook = Menu.DoYouWantTo("confirm this booking");

            if (willBook) {
                (bool isSaved, Reservation? finalBooking) = FlightReservation.CreateBooking(newReservation);
            
                if (isSaved && finalBooking != null)
                {
                    Console.Clear();
                    DisplayReservation(finalBooking);
                    Console.WriteLine("\n\nBooking is successfully created! Press any key... \n");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("\n\nBooking failed. Please try again. Press any key... \n");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("\n\nBooking canceled! Press any key...");
                Console.ReadKey();
                MenuUI();
            }


            var rerun = Menu.DoYouWantTo("create another reservation");
            if (rerun) { CreateReservationUI(); }
            else { MenuUI(); }
        }

        private static (bool flightIsAvailable, Flight? flightSelected) SearchFlightAvailabilityUI()
        {
            var airlineCode = ReturnValidValue.IfInputIsValid(FlightField.AirlineCode, MenuUI);
            var flightNumber = int.Parse(ReturnValidValue.IfInputIsValid(FlightField.FlightNumber, MenuUI)!);
            
            var searchedFlights = FlightMaintenance.SearchByAirlineCodeAndFlightNumber(airlineCode!, flightNumber);

            if (searchedFlights?.Count() < 1 || searchedFlights == null)
            {
                ConsoleExtension.ClearConsecutiveFields();
                Console.Write("No flights with these parameters are available. Press any key...");
                Console.ReadKey();
                ConsoleExtension.ClearMessageForTwoConsecutiveFields();
                return (flightIsAvailable: false, flightSelected: null);
            }

            FlightMaintenanceUI.DisplayFlights(searchedFlights!);

            Console.WriteLine("\n");
            var chosenFlight = int.Parse(ReturnValidValue.IfInputIsValid("Choose a flight", searchedFlights?.Count(), MenuUI)!);
            
            return (flightIsAvailable: true, flightSelected: searchedFlights?.ElementAt(chosenFlight - 1));
        }

        private static (DateTime? flightDate, int numberOfPassengers, List<Passenger>? passengerList) AskReservationDetails(Flight? flightDetails)
        {
            var initialFlightDate = DateTime.Parse(ReturnValidValue.IfInputIsValid(ReservationField.FlightDate, MenuUI)!);
            var numberOfPassengers = int.Parse(ReturnValidValue.IfInputIsValid(ReservationField.NumberOfPassengers, MenuUI)!);

            var flightDate = initialFlightDate.Date.AddHours(flightDetails!.ScheduledTimeOfDeparture!.Value.Hour)
                                                   .AddMinutes(flightDetails.ScheduledTimeOfDeparture.Value.Minute);

            var passengerList = new List<Passenger>();
            
            Console.WriteLine("\nPassenger Details - \n");

            for (int x = 0; x < numberOfPassengers; x++)
            {
                Console.WriteLine($"[{x + 1}]");
                var firstName = ReturnValidValue.IfInputIsValid(PassengerField.FirstName, MenuUI);
                var lastName = ReturnValidValue.IfInputIsValid(PassengerField.LastName, MenuUI);
                var birthDate = DateTime.Parse(ReturnValidValue.IfInputIsValid(PassengerField.BirthDate, MenuUI)!);
                Console.Write("\n");

                Passenger passenger = new()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    BirthDate = birthDate
                };

                passengerList.Add(passenger);
            }
          
            return (flightDate, numberOfPassengers, passengerList);
        }

        private static void ReservationSummary(Reservation reservation)
        {
            Console.Clear();
            Console.WriteLine("Reservation Details: \n");
            Console.Write("Airline Code \t");
            Console.Write("Flight Number \t");
            Console.Write("Departure Station \t");
            Console.Write("Arrival Station \t");
            Console.Write("Flight Date \t");
            Console.Write("# of Passengers \t");

            Console.WriteLine("\n");
            Console.Write($"{reservation.FlightDetails?.AirlineCode} \t\t");
            Console.Write($"{reservation.FlightDetails?.FlightNumber} \t\t");
            Console.Write($"{reservation.FlightDetails?.DepartureStation} \t\t\t");
            Console.Write($"{reservation.FlightDetails?.ArrivalStation} \t\t\t");
            Console.Write($"{reservation.FlightDate!.Value:MM/dd/yyyy} \t\t");
            Console.Write($"{reservation.NumberOfPassengers}");

            Console.WriteLine("\n");
            Console.WriteLine($"Passenger/s: ");

            for (int x = 0; x < reservation.NumberOfPassengers; x++)
            {
                Console.WriteLine("\n");
                Console.WriteLine($"[{x + 1}]");
                Console.WriteLine($"Name: {reservation.ListOfPassengers?[x].Name}");
                Console.WriteLine($"Date of Birth: {reservation.ListOfPassengers?[x].BirthDate.ToShortDateString()}");
                var age = reservation.ListOfPassengers?[x].Age < 1 ? "0 (Infant)" : reservation.ListOfPassengers?[x].Age.ToString();
                Console.WriteLine($"Age: {age}");
            }
            
        }

        private static void ListAllReservationUI()
        {
            Console.Clear();
            Console.Title = "Reservations - All Reservations";

            var allReservations  = FlightReservation.GetAllReservations();

            if (allReservations?.Count() > 0)
            {
                Console.Write("#\t");
                Console.Write("PNR Number \t");
                Console.Write("Flight Date \t");
                Console.Write("Airline Code \t");
                Console.Write("Flight Number \t");
                Console.Write("Origin \t");
                Console.Write("Destination \t");
                Console.Write("# of Passengers \t");

                for (int x = 0; x < allReservations?.Count(); x++)
                {
                    if(x%10 == 0 && x != 0) 
                    {
                        var breakFromLoop = Menu.DoYouWantTo("see more reservations");
                        ConsoleExtension.ClearMessage();
                        Console.SetCursorPosition(0, Console.CursorTop - 2);
                        if (!breakFromLoop) 
                        {
                            break; 
                        }
                    }

                    Console.WriteLine("\n");
                    Console.Write($"[{x + 1}]\t");
                    Console.Write($"{allReservations?.ElementAt(x).PNRNumber} \t\t");
                    Console.Write($"{allReservations?.ElementAt(x).FlightDate!.Value.ToShortDateString()} \t");
                    Console.Write($"{allReservations?.ElementAt(x).FlightDetails?.AirlineCode} \t\t");
                    Console.Write($"{allReservations?.ElementAt(x).FlightDetails?.FlightNumber} \t\t");
                    Console.Write($"{allReservations?.ElementAt(x).FlightDetails?.DepartureStation} \t");
                    Console.Write($"{allReservations?.ElementAt(x).FlightDetails?.ArrivalStation} \t\t\t");
                    Console.Write($"{allReservations?.ElementAt(x).NumberOfPassengers} \t");
                }

                Console.WriteLine("\n\n\n");
                var chosenReservation = int.Parse(ReturnValidValue.IfInputIsValid("Choose a reservation to display", allReservations?.Count(), MenuUI)!);
                Console.Clear();
                DisplayReservation(allReservations!.ElementAt(chosenReservation - 1));
                
                var rerun = Menu.DoYouWantTo("see the list of reservations again");
                if (rerun) { ListAllReservationUI(); }
                else { MenuUI(); }
            }
            else
            {
                Console.WriteLine("RESPONSE: No reservations available. Press any key...");
                Console.ReadKey();
                MenuUI();
            }
        }

        private static void SearchReservationUI()
        {
            Console.Clear();
            Console.Title = "Reservations - Search a booking";
            Console.WriteLine("Please enter a valid PNR number!");
            ConsoleExtension.ConsoleExitHeader();

            var pnrNumber = ReturnValidValue.IfInputIsValid(ReservationField.PNRNumber, MenuUI);

            var searchedReservation = FlightReservation.GetReservation(pnrNumber!);

            if (searchedReservation != null)
            {
                DisplayReservation(searchedReservation);
            }
            else
            {
                Console.WriteLine("The entered PNR number doesn't correspond to any reservation in the database.");
            }

            var rerun = Menu.DoYouWantTo("search for another booking");
            if (rerun) { SearchReservationUI(); }
            else { MenuUI(); }
        }

        public static void DisplayReservation(Reservation reservation)
        {
            Console.WriteLine("\n\nReservation Details - \n");
            Console.WriteLine($"PNR Number: \t\t{reservation.PNRNumber}");
            Console.WriteLine($"Flight Date: \t\t{reservation.FlightDate!.Value.ToShortDateString()}");

            Console.WriteLine("\nFlight Details - \n");
            Console.WriteLine($"Airline Code: \t\t\t\t{reservation.FlightDetails?.AirlineCode} ");
            Console.WriteLine($"Flight Number: \t\t\t\t{reservation.FlightDetails?.FlightNumber}");
            Console.WriteLine($"Departure Station: \t\t\t{reservation.FlightDetails?.DepartureStation}");
            Console.WriteLine($"Arrival Station: \t\t\t{reservation.FlightDetails?.ArrivalStation}");
            Console.WriteLine($"Scheduled Time of Departure (STD): \t{reservation.FlightDetails?.ScheduledTimeOfDeparture!.Value.ToString("HH:mm")}");
            Console.WriteLine($"Scheduled Time of Arrival (STA): \t{reservation.FlightDetails?.ScheduledTimeOfArrival!.Value.ToString("HH:mm")}");

            Console.WriteLine("\nPassenger Details - \n");
            Console.WriteLine($"# of Passengers: \t{reservation.NumberOfPassengers} \n");
            
            for (int x = 0; x < reservation.NumberOfPassengers; x++)
            {
                Console.WriteLine($"[{x + 1}]:");
                Console.WriteLine($"Name: \t\t\t{reservation.ListOfPassengers?[x].Name}");
                Console.WriteLine($"Date of Birth: \t\t{reservation.ListOfPassengers?[x].BirthDate.ToShortDateString()}");
                
                var age = reservation.ListOfPassengers?[x].Age < 1 ? "0 (Infant)" : reservation.ListOfPassengers?[x].Age.ToString();
                Console.WriteLine($"Age: \t\t\t{age}");
                Console.Write("\n");
            }
        }

    }
}
