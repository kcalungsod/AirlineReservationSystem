using Navitaire.AirlineReservationSystem.Core.Common;
using Navitaire.AirlineReservationSystem.UI.Common;

namespace Navitaire.AirlineReservationSystem.Test.InputValidatorTests
{
    public class CommonValidatorTests
    {
        [Theory]
        [InlineData("1", 1)]
        [InlineData("1", 5)]
        [InlineData("1000", 10000)]
        public void ShouldReturnValid_CommonNumberInput(string input, int maxCount)
        {
            var (isValid, _) = InputValidators.ValidateCommonNumberInput(input, maxCount);

            Assert.True(isValid);
        }

        [Theory]
        [InlineData("", 1)]
        [InlineData(null, 1)]
        [InlineData("abcdefgh", 1)]
        [InlineData("/-$*#", 1)]
        [InlineData("/ab", 1)]
        [InlineData("0abcd", 1)]
        [InlineData("12asd@", 1)]
        [InlineData("0", 1)]
        [InlineData("-1", 1)]
        [InlineData("0.5", 1)]
        [InlineData("1/2", 1)]
        [InlineData("6", 5)]
        public void ShouldReturnInvalid_CommonNumberInput(string input, int maxCount)
        {
            var (isValid, _) = InputValidators.ValidateCommonNumberInput(input, maxCount);
            Assert.False(isValid);
        }

        [Theory]
        [InlineData("12345")]
        [InlineData("abcdefg")]
        public void ShouldReturnValid_CommonErrorCheck(string input)
        {
            var (isValid, _) = InputValidators.CommonErrorChecks("Common Field", input);
            var isValidJSON = ServerValidators.CommonErrorChecks(input);

            Assert.True(isValid);
            Assert.True(isValidJSON);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("!@#$%%^^&*()_+")]
        [InlineData("!@#abcd")]
        [InlineData("!@#12345")]
        public void ShouldReturnInvalid_CommonErrorCheck(string input)
        {
            var (isValid, _) = InputValidators.CommonErrorChecks("Common Field", input);
            var isValidJSON = ServerValidators.CommonErrorChecks(input);

            Assert.False(isValid);
            Assert.False(isValidJSON);
        }
    }
}
