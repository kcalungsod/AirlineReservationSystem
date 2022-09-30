using Navitaire.AirlineReservationSystem.Core.Models;
using Navitaire.AirlineReservationSystem.Core.Services;

namespace Navitaire.AirlineReservationSystem.Test.ServiceTests
{
    public class FlightMaintenanceFixture: IDisposable
    { 
        public FlightMaintenanceFixture()
        {
            FlightMaintenance.DatabaseType = "TESTDB";
        }

        public void Dispose()
        {
            FlightMaintenance.DatabaseType = "JSONDB";
        }
    }

    public class FlightMaintenanceServices: IClassFixture<FlightMaintenanceFixture>
    {
        FlightMaintenanceFixture flightMaintenanceFixture;
        public FlightMaintenanceServices(FlightMaintenanceFixture flightMaintenanceFixture)
        {
            this.flightMaintenanceFixture = flightMaintenanceFixture;   
        }

        public static IEnumerable<object[]> GetMockFlightsForSearch =>
        new List<object[]>
        {
            new Flight[] { 
                new Flight
                {
                    AirlineCode = "5J", FlightNumber = 1000, DepartureStation = "MNL", ArrivalStation = "CEB"
                },
                new Flight
                {
                    AirlineCode = "5J", FlightNumber = 2000, DepartureStation = "DVO", ArrivalStation = "CEB"
                },
                new Flight
                {
                    AirlineCode = "4k", FlightNumber = 5000, DepartureStation = "CEB", ArrivalStation = "MNL"
                },
                new Flight
                {
                    AirlineCode = "NV", FlightNumber = 5000, DepartureStation = "MNL", ArrivalStation = "CEB"
                },
                new Flight
                {
                    AirlineCode = "NV", FlightNumber = 5000, DepartureStation = "CEB", ArrivalStation = "MNL"
                }
            }
        };


        [Theory]
        [MemberData(nameof(GetMockFlightsForSearch))]
        public void ShouldReturnCorrectFlights_SearchByAirlineCode(params Flight[] mockFlights)
        {
            //Arrange
            foreach(Flight flight in mockFlights)
            {
                FlightMaintenance.AddFlight(flight);
            }

            IEnumerable<Flight> expectedFlights = new List<Flight>
            {
                new Flight
                {
                    AirlineCode = "5J", FlightNumber = 1000, DepartureStation = "MNL", ArrivalStation = "CEB"
                },
                new Flight
                {
                    AirlineCode = "5J", FlightNumber = 2000, DepartureStation = "DVO", ArrivalStation = "CEB"
                }
            };

            var airlineCode = "5J";

            //Act
            var flightsSearched = FlightMaintenance.SearchByAirlineCode(airlineCode);

         
            //Assert
            Assert.Equal(expectedFlights.Count(), flightsSearched!.Count());
            Assert.Equivalent(expectedFlights, flightsSearched);    
        }

        [Theory]
        [MemberData(nameof(GetMockFlightsForSearch))]
        public void ShouldReturnNull_SearchByAirlineCode(params Flight[] mockFlights)
        {
            //Arrange
            foreach (Flight flight in mockFlights)
            {
                FlightMaintenance.AddFlight(flight);
            }

            var airlineCode = "AD";
            
            //Act
            var flightsSearched = FlightMaintenance.SearchByAirlineCode(airlineCode);

            //Assert
            Assert.Equal(0, flightsSearched?.Count());
            Assert.Empty(flightsSearched!);
        }

        [Theory]
        [MemberData(nameof(GetMockFlightsForSearch))]
        public void ShouldReturnCorrectFlights_SearchByFlightNumber(params Flight[] mockFlights)
        {
            //Arrange
            foreach (Flight flight in mockFlights)
            {
                FlightMaintenance.AddFlight(flight);
            }

            IEnumerable<Flight> expectedFlights = new List<Flight>
            {
                new Flight
                {
                    AirlineCode = "4k", FlightNumber = 5000, DepartureStation = "CEB", ArrivalStation = "MNL"
                },
                new Flight
                {
                    AirlineCode = "NV", FlightNumber = 5000, DepartureStation = "MNL", ArrivalStation = "CEB"
                },
                new Flight
                {
                    AirlineCode = "NV", FlightNumber = 5000, DepartureStation = "CEB", ArrivalStation = "MNL"
                }
            };

            var flightNumber = 5000;

            //Act
            var flightsSearched = FlightMaintenance.SearchByFlightNumber(flightNumber);

            //Assert
            Assert.Equal(expectedFlights.Count(), flightsSearched!.Count());
            Assert.Equivalent(expectedFlights, flightsSearched);
        }

        [Theory]
        [MemberData(nameof(GetMockFlightsForSearch))]
        public void ShouldReturnNull_SearchByFlightNumber(params Flight[] mockFlights)
        {
            //Arrange
            foreach (Flight flight in mockFlights)
            {
                FlightMaintenance.AddFlight(flight);
            }

            var flightNumber = 1234;

            //Act
            var flightsSearched = FlightMaintenance.SearchByFlightNumber(flightNumber);

            //Assert
            Assert.Equal(0, flightsSearched?.Count());
            Assert.Empty(flightsSearched!);
        }

        [Theory]
        [MemberData(nameof(GetMockFlightsForSearch))]
        public void ShouldReturnCorrectFlights_SearchByMarket(params Flight[] mockFlights)
        {
            //Arrange
            foreach (Flight flight in mockFlights)
            {
                FlightMaintenance.AddFlight(flight);
            }

            IEnumerable<Flight> expectedFlights = new List<Flight>
            {
                new Flight
                {
                    AirlineCode = "5J", FlightNumber = 1000, DepartureStation = "MNL", ArrivalStation = "CEB"
                },
                new Flight
                {
                    AirlineCode = "NV", FlightNumber = 5000, DepartureStation = "MNL", ArrivalStation = "CEB"
                }
            };

            var departureStation = "MNL";
            var arrivalStation = "CEB";

            //Act
            var flightsSearched = FlightMaintenance.SearchByMarket(departureStation, arrivalStation);

            //Assert
            Assert.Equal(expectedFlights.Count(), flightsSearched!.Count());
            Assert.Equivalent(expectedFlights, flightsSearched);
        }

        [Theory]
        [MemberData(nameof(GetMockFlightsForSearch))]
        public void ShouldReturnNull_SearchByMarket(params Flight[] mockFlights)
        {
            //Arrange
            foreach (Flight flight in mockFlights)
            {
                FlightMaintenance.AddFlight(flight);
            }

            var departureStation = "MNL";
            var arrivalStation = "SIN";

            //Act
            var flightsSearched = FlightMaintenance.SearchByMarket(departureStation, arrivalStation);

            //Assert
            Assert.Empty(flightsSearched!);
        }

        [Theory]
        [MemberData(nameof(GetMockFlightsForSearch))]
        public void ShouldReturnCorrectFlights_SearchByAirlineCodeAndFlightNumber(params Flight[] mockFlights)
        {
            //Arrange
            foreach (Flight flight in mockFlights)
            {
                FlightMaintenance.AddFlight(flight);
            }

            IEnumerable<Flight> expectedFlights = new List<Flight>
            {
                new Flight
                {
                    AirlineCode = "NV", FlightNumber = 5000, DepartureStation = "MNL", ArrivalStation = "CEB"
                },
                new Flight
                {
                    AirlineCode = "NV", FlightNumber = 5000, DepartureStation = "CEB", ArrivalStation = "MNL"
                }
            };

            var airlineCode = "NV";
            var flightNumber = 5000;

            //Act
            var flightsSearched = FlightMaintenance.SearchByAirlineCodeAndFlightNumber(airlineCode, flightNumber);

            //Assert
            Assert.Equal(expectedFlights.Count(), flightsSearched!.Count());
            Assert.Equivalent(expectedFlights, flightsSearched);
        }

        [Theory]
        [MemberData(nameof(GetMockFlightsForSearch))]
        public void ShouldReturnNull_SearchByAirlineCodeAndFlightNumber(params Flight[] mockFlights)
        {
            //Arrange
            foreach (Flight flight in mockFlights)
            {
                FlightMaintenance.AddFlight(flight);
            }

            var airlineCode = "NV";
            var flightNumber = 1000;

            //Act
            var flightsSearched = FlightMaintenance.SearchByAirlineCodeAndFlightNumber(airlineCode, flightNumber);

            //Assert
            Assert.Equal(0, flightsSearched?.Count());
            Assert.Empty(flightsSearched!);
        }

        public static IEnumerable<object[]> GetMockFlightsForAdd(int numTests)
        {
            var allFlights = new List<object[]>
            {
                new Flight[] {
                    new Flight
                    {
                        AirlineCode = "NV", FlightNumber = 5000, DepartureStation = "MNL", ArrivalStation = "CEB"
                    },
                    new Flight
                    {
                        AirlineCode = "NV", FlightNumber = 5000, DepartureStation = "CEB", ArrivalStation = "MNL"
                    },
                    new Flight
                    {
                        AirlineCode = "NV", FlightNumber = 5000, DepartureStation = "CEB", ArrivalStation = "MNL"
                    }

                }
            };

            return allFlights.Take(numTests);
        }

        [Theory]
        [MemberData(nameof(GetMockFlightsForAdd), parameters: 2)]
        public void ShouldPass_WhenAddingUniqueFlights(params Flight[] mockFlights)
        {
            bool[] results = Array.Empty<bool>();

            //Act
            foreach(var flight in mockFlights)
            {
                var response = FlightMaintenance.AddFlight(flight);
                results.Append(response);
            }

            //Assert
            foreach(var result in results)
            {
                Assert.True(result);
            }
        }

        [Theory]
        [MemberData(nameof(GetMockFlightsForAdd), parameters: 3)]
        public void ShouldFail_WhenAddingDuplicateFlights(params Flight[] mockFlights)
        {
            bool[] results = Array.Empty<bool>();

            //Act
            foreach (var flight in mockFlights)
            {
                var response = FlightMaintenance.AddFlight(flight);
                results.Append(response);
            }

            //Assert
            for (int x = 0; x < results.Length; x++)
            {
                if (x != 2)
                {
                    Assert.True(results[x]);
                }
                else
                {
                    Assert.False(results[x]);
                }
            }
        }

        
    }
}
