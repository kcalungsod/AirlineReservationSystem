namespace Navitaire.AirlineReservationSystem.Core.Common
{
    public static class StringExtension
    {
        public static bool HasSpecialChars(this string receivedString)
        {
            return receivedString.Any(letter => !char.IsLetterOrDigit(letter));
        }

    }
}
