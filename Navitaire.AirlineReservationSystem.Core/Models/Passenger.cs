namespace Navitaire.AirlineReservationSystem.Core.Models
{
    public class Passenger
    {
        private string? _firstName;
        private string? _lastName;
        
        public string? FirstName
        {
            get => _firstName;
            set => _lastName = value?.ToUpper();
        }
        public string? LastName
        {
            get => _lastName;
            set => _lastName = value?.ToUpper();
        }
        public string Name
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public DateTime BirthDate { get; set; } = DateTime.Now;
        public int Age => CalculateAge();

        public int CalculateAge()
        {
            DateTime today = DateTime.Now;
            DateTime birthDay = BirthDate;

            int age = today.Year - birthDay.Year;
            if (birthDay.Month > today.Month ||
                birthDay.Month == today.Month && birthDay.Day > today.Day)
            {
                age--;
            }
            if (birthDay > today.AddYears(-age)) { age--; }

            return age;
        }

    }
}
