using Navitaire.AirlineReservationSystem.Core.Common;
using Navitaire.AirlineReservationSystem.Core.Models;
using Navitaire.AirlineReservationSystem.Core.Services;

namespace Navitaire.AirlineReservationSystem.Test.ServiceTests
{
    public class ReservationFixture : IDisposable
    {
        public ReservationFixture()
        {
            FlightReservation.DatabaseType = "TESTDB";
        }

        public void Dispose()
        {
            FlightReservation.DatabaseType = "JSONDB";
        }
    }

    public class ReservationServices: IClassFixture<ReservationFixture>
    {
        ReservationFixture reservationFixture;

        public ReservationServices(ReservationFixture reservationFixture)
        {
            this.reservationFixture = reservationFixture;
        }

        public static IEnumerable<object[]> GetAgeTestData =>
        new List<object[]>
        {
            new object[] {"1/1/1850", 172},
            new object[] {"1/1/1980", 42},
            new object[] {"1/1/2000", 22},
            new object[] {"2/29/2020", 2},
            new object[] {"1/1/2022", 0},
        };

        [Theory]
        [MemberData(nameof(GetAgeTestData))]
        public void ShouldReturnCorrectAge(string birthDate, int expectedAge)
        {
            var parsedBirthDate = DateTime.Parse(birthDate);

            Passenger passenger = new();
            passenger.BirthDate = parsedBirthDate;

            Assert.True(passenger.Age == expectedAge);
        }

        [Fact]
        public void ShouldReturnUniquePNR()
        {
            List<string> pnrList = new();
            
            for(var x = 0; x < 100; x++)
            {
                var newPNR = FlightReservation.GeneratePNRNumber();
                pnrList.Add(newPNR!);
            }

            Assert.Distinct(pnrList);
        }

        [Fact]
        public void ShouldCheckIfPNRExistsInDB()
        {
            Reservation reservation = new Reservation();

            var pnrNumber = FlightReservation.CreateBooking(reservation).finalBooking?.PNRNumber;
            
            var pnrExistsInDB = FlightReservation.CheckIfPNRNumberExists(pnrNumber!);

            Assert.True(pnrExistsInDB);
        }

        [Fact]
        public void ShouldReturnCorrectReservation()
        {
            Reservation reservation = new()
            {
                FlightDetails = new Flight
                {
                    AirlineCode = "5J", FlightNumber = 1000, DepartureStation = "MNL", ArrivalStation = "CEB"
                },
                FlightDate = DateTime.Parse("12/1/2022"),
                NumberOfPassengers = 1,
                ListOfPassengers = new List<Passenger>() 
                { 
                    new Passenger { FirstName = "Kaye", LastName = "Calungsod" },
                }
            };

            var pnrNumber = FlightReservation.CreateBooking(reservation).finalBooking?.PNRNumber;

            reservation.PNRNumber = pnrNumber;
            var actualResult = FlightReservation.GetReservation(pnrNumber!);

            Assert.Equivalent(reservation, actualResult);
        }

        [Fact]
        public void ShouldReturnNoReservation()
        {
            Reservation reservation = new()
            {
                FlightDetails = new Flight
                {
                    AirlineCode = "5J",
                    FlightNumber = 1000,
                    DepartureStation = "MNL",
                    ArrivalStation = "CEB"
                },
                FlightDate = DateTime.Parse("12/1/2022"),
                NumberOfPassengers = 1,
                ListOfPassengers = new List<Passenger>()
                {
                    new Passenger { FirstName = "Kaye", LastName = "Calungsod" },
                }
            };

            FlightReservation.CreateBooking(reservation);

            var randomPNRNumber = "ABCDEF";

            var actualResult = FlightReservation.GetReservation(randomPNRNumber);

            Assert.Null(actualResult);
        }

        [Fact]
        public void ShouldReturnAllReservations()
        {
            TestDB.CurrentReservations.Clear();

            for (int x = 0; x < 5; x++)
            {
                FlightReservation.CreateBooking(new Reservation());
            }

            var allReservations = FlightReservation.GetAllReservations();

            Assert.Equal(5, allReservations?.Count());
        }

        [Fact]
        public void ShouldReturnNullReservations()
        {
            TestDB.CurrentReservations.Clear();

            var allReservations = FlightReservation.GetAllReservations();

            Assert.Empty(allReservations!);
        }

    }
}
