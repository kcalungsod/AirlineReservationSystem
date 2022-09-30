namespace Navitaire.AirlineReservationSystem.UI.Common
{
    public class Option
    {
        public Option(string? name, Action? method)
        {
            Name = name;
            Method = method;
        }

        public string? Name { get; set; }
        public Action? Method { get; set; }
    }
}
