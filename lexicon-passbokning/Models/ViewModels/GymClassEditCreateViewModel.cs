using lexicon_passbokning.Validation;
using System.ComponentModel;

namespace lexicon_passbokning.Models.ViewModels
{
    public class GymClassEditCreateViewModel
    {
        public int Id { get; set; }

        [DisplayName("Gym Class Title")]
        public string Name { get; set; }

        public DateTime StartTime { get; set; }

        [DurationInRange(ErrorMessage = "Please enter a valid duration between 00:00 and 24:00.")]
        [DisplayName("The lesson duration (hh:mm)")]
        public TimeSpan Duration { get; set; }

        public String Description { get; set; }
    }
}
