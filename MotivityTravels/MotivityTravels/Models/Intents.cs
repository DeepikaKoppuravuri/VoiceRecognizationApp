global using Newtonsoft.Json;

namespace MotivityTravels.Models
{
    public class Intents
    {
        public Location Location { get; set; }
        public Dates Dates { get; set; }
        public Persons persons { get; set; }
    }
}
