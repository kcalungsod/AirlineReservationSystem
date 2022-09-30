namespace Navitaire.AirlineReservationSystem.Core.Models
{
    public class FlightList
    {
        public Flight[]? FlightDB;
    }

    public class Flight
    {
        private string? _airlineCode;
        private string?_arrivalStation;
        private string? _departureStation;


        public string? AirlineCode
        {
            get => _airlineCode;
            set => _airlineCode = value?.ToUpper();
        }
        public int? FlightNumber { get; set; }
        public string? DepartureStation 
        {
            get => _departureStation; 
            set => _departureStation = value?.ToUpper(); 
        }
        public string? ArrivalStation
        {
            get => _arrivalStation;
            set => _arrivalStation = value?.ToUpper();
        }
        public DateTime? ScheduledTimeOfDeparture { get; set; }
        public DateTime? ScheduledTimeOfArrival { get; set; }
    }
}
