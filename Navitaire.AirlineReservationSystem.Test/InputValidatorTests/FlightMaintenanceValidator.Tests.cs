using Navitaire.AirlineReservationSystem.Core.Common;
using Navitaire.AirlineReservationSystem.UI.Common;

namespace Navitaire.AirlineReservationSystem.Test.InputValidatorTests
{
    public class FlightMaintenanceValidator
    {

        [Theory]
        [InlineData("5j")]
        [InlineData("5J")]
        [InlineData("AB")]
        [InlineData("ab")]
        [InlineData("Ab")]
        public void ShouldReturnValid_ValidateAirlineCode(string input)
        {
            var (isValid, _) = InputValidators.ValidateAirlineCode(input);
            var isValidJSON = ServerValidators.ValidateAirlineCode(input);
            Assert.True(isValid);
            Assert.True(isValidJSON);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("5")]
        [InlineData("J")]
        [InlineData("j")]
        [InlineData("55")]
        [InlineData("J5")]
        [InlineData("5A5")]
        [InlineData("ABC")]
        [InlineData("abc")]
        [InlineData("/*@")]
        [InlineData("1/")]
        [InlineData("J@")]
        public void ShouldReturnInvalid_ValidateAirlineCode(string input)
        {
            var (isValid, _) = InputValidators.ValidateAirlineCode(input);
            var isValidJSON = ServerValidators.ValidateAirlineCode(input);

            Assert.False(isValid);
            Assert.False(isValidJSON);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("9999")]
        [InlineData("0001")]
        [InlineData("0010")]
        [InlineData("0100")]
        [InlineData("1000")]
        public void ShouldReturnValid_ValidateFlightNumber(string input)
        {
            var (isValid, _) = InputValidators.ValidateFlightNumber(input);
            var isValidJSON = ServerValidators.ValidateFlightNumber(input);

            Assert.True(isValid);
            Assert.True(isValidJSON);
        }


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("0")]
        [InlineData("-1")]
        [InlineData("0.5")]
        [InlineData("1/2")]
        [InlineData("10000")]
        [InlineData("abc")]
        [InlineData("/@-!")]
        [InlineData("100a")]
        [InlineData("a100!")]
        public void ShouldReturnInvalid_ValidateFlightNumber(string input)
        {
            var (isValid, _) = InputValidators.ValidateFlightNumber(input);
            var isValidJSON = ServerValidators.ValidateFlightNumber(input);
            
            Assert.False(isValid);
            Assert.False(isValidJSON);
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("a12")]
        [InlineData("bc1")]
        [InlineData("ABC")]
        [InlineData("A12")]
        [InlineData("BC1")]
        public void ShouldReturnValid_ValidateStationCode(string input)
        {
            var (isValid, _) = InputValidators.ValidateStation(input, "Code");
            var isValidJSON = ServerValidators.ValidateStation(input);
            
            Assert.True(isValid);
            Assert.True(isValidJSON);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("0")]
        [InlineData("-1")]
        [InlineData("0.5")]
        [InlineData("1/2")]
        [InlineData("123")]
        [InlineData("12A")]
        [InlineData("1234")]
        [InlineData("a")]
        [InlineData("A")]
        [InlineData("AB")]
        [InlineData("Ab")]
        [InlineData("abcd")]
        [InlineData("ABCD")]
        [InlineData("/@-!")]
        [InlineData("a100!")]
        public void ShouldReturnInvalid_ValidateStationCode(string input)
        {
            var (isValid, _) = InputValidators.ValidateStation(input, "Code");            
            var isValidJSON = ServerValidators.ValidateStation(input);

            Assert.False(isValid);
            Assert.False(isValidJSON);
        }

        [Theory]
        [InlineData("MNL", "MNL")]
        [InlineData("mnl", "MNL")]
        [InlineData("CEB", "ceb")]
        [InlineData("ceb", "ceb")]
        public void ShouldReturnInvalid_ValidateDepartureAndArrivalStation(string origin, string destination)
        {
            var (isValid, _) = InputValidators.ValidateDepartureAndArrivalStation(origin, destination);
            var isValidJSON = ServerValidators.ValidateDepartureAndArrivalStation(origin, destination);

            Assert.False(isValid);
            Assert.False(isValidJSON);
        }

        [Theory]
        [InlineData("0000")]
        [InlineData("1000")]
        [InlineData("1300")]
        [InlineData("2359")]
        public void ShouldReturnValid_ValidateScheduledTime(string input)
        {
            var (isValid, _) = InputValidators.ValidateScheduledTime(input, "Code");
            Assert.True(isValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("0")]
        [InlineData("-1")]
        [InlineData("0.5")]
        [InlineData("1/2")]
        [InlineData("12A")]
        [InlineData("a")]
        [InlineData("A")]
        [InlineData("AB")]
        [InlineData("Ab")]
        [InlineData("abcd")]
        [InlineData("ABCD")]
        [InlineData("/@-!")]
        [InlineData("a100!")]
        [InlineData("930")]
        [InlineData("2400")]
        [InlineData("0060")]
        [InlineData("1279")]
        [InlineData("12300")]
        [InlineData("12:30")]
        [InlineData("13.30")]
        [InlineData("13 30")]
        public void ShouldReturnInvalid_ValidateScheduledTime(string input)
        {
            var (isValid, _) = InputValidators.ValidateScheduledTime(input, "Station Code");

            Assert.False(isValid);
        }

    }
}