namespace lexicon_passbokning.Models.ViewModels
{
    public class GymClassBookingViewModel
    {
        public GymClass GymClass { get; set; }
        public bool IsBooked { get; set; } = false;
    }
}
