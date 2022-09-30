using Navitaire.AirlineReservationSystem.Core.Common;
using Navitaire.AirlineReservationSystem.UI.Common;

namespace Navitaire.AirlineReservationSystem.Test.InputValidatorTests
{
    public class ReservationValidator
    {

        [Theory]
        [InlineData("10/1")]
        [InlineData("10/1/23")]
        [InlineData("10/1/2023")]
        [InlineData("11-1-2023")]
        [InlineData("11.1.2023")]
        [InlineData("11 1 2023")]
        public void ShouldReturnValid_ValidateFlightDate(string input)
        {
            var (isValid, _) = InputValidators.ValidateFlightDate(input);
            var isValidJSON = ServerValidators.ValidateFlightDate(input);


            Assert.True(isValid);
            Assert.True(isValidJSON);
        }

        [Fact]
        public void ShouldReturnValidIfInputIsDateToday_ValidateFlightDate()
        {
            var dateToday = DateTime.Now.Date.ToString("MM/dd/yyyy");
            var (isValid, _) = InputValidators.ValidateFlightDate(dateToday);
            var isValidJSON = ServerValidators.ValidateFlightDate(dateToday);


            Assert.True(isValid);
            Assert.True(isValidJSON);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("0")]
        [InlineData("-1")]
        [InlineData("0.5")]
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
        [InlineData("9/1")]
        [InlineData("9/20/22")]
        [InlineData("10/1/2020")]
        public void ShouldReturnInvalid_ValidateFlightDate(string input)
        {
            var (isValid, _) = InputValidators.ValidateFlightDate(input);
            var isValidJSON = ServerValidators.ValidateFlightDate(input);

            Assert.False(isValid);
            Assert.False(isValidJSON);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("3")]
        [InlineData("5")]
        public void ShouldReturnValid_ValidatePassengerCount(string input)
        {
            var (isValid, _) = InputValidators.ValidatePassengerCount(input);

            Assert.True(isValid);
        }

        [Theory]
        [InlineData("1", 1)]
        [InlineData("3", 3)]
        [InlineData("5", 5)]
        public void ShouldReturnValid_ValidateJSONPassengerCount(string input, int listOfPassengerCount)
        {
            var isValidJSON = ServerValidators.ValidatePassengerCount(input, listOfPassengerCount);

            Assert.True(isValidJSON);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("0")]
        [InlineData("-1")]
        [InlineData("0.5")]
        [InlineData("1/2")]
        [InlineData("6")]
        [InlineData("123")]
        [InlineData("12A")]
        [InlineData("1234")]
        [InlineData("abcd")]
        [InlineData("ABCD")]
        [InlineData("/@-!")]
        [InlineData("a100!")]
        public void ShouldReturnInvalid_ValidatePassengerCount(string input)
        {
            var (isValid, _) = InputValidators.ValidatePassengerCount(input);

            Assert.False(isValid);
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData(null, 0)]
        [InlineData("0", 0)]
        [InlineData("-1", 0)]
        [InlineData("0.5", 1)]
        [InlineData("1/2", 1)]
        [InlineData("6", 6)]
        [InlineData("1", 2)]
        [InlineData("3", 4)]
        [InlineData("5", 6)]
        [InlineData("123", 123)]
        public void ShouldReturnInvalid_ValidateJSONPassengerCount(string input, int listOfPassengerCount)
        {
            var isValidJSON = ServerValidators.ValidatePassengerCount(input, listOfPassengerCount);

            Assert.False(isValidJSON);
        }

        [Theory]
        [InlineData("abcd")]
        [InlineData("ABCD")]
        [InlineData("Abcd")]
        [InlineData("Abcd-Efghi")]
        [InlineData("Abcd Efghi Jklmnopqr")]
        [InlineData("Abcdefghijklmnopqrst")]
        [InlineData("Abc III")]
        [InlineData("Abc JR.")]
        [InlineData("Abc 4th")]
        [InlineData("Abc sr.")]
        [InlineData("熙 凤")]
        [InlineData("에스파")]
        [InlineData("山нσ∂υѕѕιѕуѕ")]
        public void ShouldReturnValid_ValidateName(string input)
        {
            var (isValid, _) = InputValidators.ValidateName(input, "Name");
            var isValidJSON = ServerValidators.ValidateName(input);

            Assert.True(isValid);
            Assert.True(isValidJSON);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("Abcd Efghi Jklmnopqrstuv Wxyz")]
        [InlineData("Abcdefghijklmnopqrstuvwxyz")]
        public void ShouldReturnInvalid_ValidateName(string input)
        {
            var (isValid, _) = InputValidators.ValidateName(input, "Name");
            var isValidJSON = ServerValidators.ValidateName(input);

            Assert.False(isValid);
            Assert.False(isValidJSON);
        }


        [Theory]
        [InlineData("1/1")]
        [InlineData("1/1/22")]
        [InlineData("9/25/22")]
        [InlineData("1-1-2022")]
        [InlineData("1.1.2022")]
        [InlineData("1 1 2022")]
        public void ShouldReturnValid_ValidateBirthDate(string input)
        {
            var (isValid, _) = InputValidators.ValidateBirthDate(input);
            var isValidJSON = ServerValidators.ValidateBirthDate(input);

            Assert.True(isValid);
            Assert.True(isValidJSON);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("0")]
        [InlineData("-1")]
        [InlineData("0.5")]
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
        [InlineData("12/1")]
        [InlineData("12/1/22")]
        [InlineData("1/1/2023")]
        [InlineData("1-1-2023")]
        [InlineData("1.1.2023")]
        [InlineData("1 1 2023")]
        public void ShouldReturnInvalid_ValidateBirthDate(string input)
        {
            var (isValid, _) = InputValidators.ValidateBirthDate(input);
            var isValidJSON = ServerValidators.ValidateBirthDate(input);

            Assert.False(isValid);
            Assert.False(isValidJSON);
        }

        [Theory]
        [InlineData("ABCDEF")]
        [InlineData("Abcdef")]
        [InlineData("abcdef")]
        [InlineData("A12345")]
        [InlineData("AB2345")]
        [InlineData("ABC345")]
        [InlineData("ABCD45")]
        [InlineData("ABCDE5")]
        public void ShouldReturnValid_ValidatePNR(string input)
        {
            var (isValid, _) = InputValidators.ValidatePNR(input);
            var isValidJSON = ServerValidators.ValidatePNR(input);

            Assert.True(isValid);
            Assert.True(isValidJSON);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("0")]
        [InlineData("-1")]
        [InlineData("0.5")]
        [InlineData("123")]
        [InlineData("12A")]
        [InlineData("1234")]
        [InlineData("a")]
        [InlineData("A")]
        [InlineData("AB")]
        [InlineData("Ab")]
        [InlineData("abcd")]
        [InlineData("ABCD")]
        [InlineData("ABCDE")]
        [InlineData("ABCDEFG")]
        [InlineData("/@-!")]
        [InlineData("a100!")]
        [InlineData("12/1")]
        [InlineData("12/1/22")]
        [InlineData("1/1/2023")]
        [InlineData("1-1-2023")]
        [InlineData("1.1.2023")]
        [InlineData("1 1 2023")]
        [InlineData("1BCDEF")]
        [InlineData("1bcdef")]
        [InlineData("12345")]
        [InlineData("A B C D E F")]
        [InlineData("ABC DEF")]
        public void ShouldReturnInvalid_ValidatePNR(string input)
        {
            var (isValid, _) = InputValidators.ValidatePNR(input);
            var isValidJSON = ServerValidators.ValidatePNR(input);

            Assert.False(isValid);
            Assert.False(isValidJSON);
        }


    }
}
