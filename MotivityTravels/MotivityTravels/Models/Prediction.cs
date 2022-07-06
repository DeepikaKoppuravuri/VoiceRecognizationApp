namespace MotivityTravels.Models
{
    public class Prediction
    {
        public string topIntent { get; set; }
        public Intents intents { get; set; }
        public Entities entities { get; set; }
    }
}
