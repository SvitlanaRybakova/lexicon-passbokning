namespace lexicon_passbokning.Models
{
    public class GymClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; } 
        public TimeSpan Duration { get; set; } 
        public DateTime EndTime { get { return StartTime + Duration; } } 
        public String Description { get; set; }

        // navigation M-M to the ApplicationUserGymClass join table
        public ICollection<ApplicationUserGymClass> AttendingClasses { get; set; } = new List<ApplicationUserGymClass>();

    }
}
