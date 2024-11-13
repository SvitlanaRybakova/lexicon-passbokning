namespace lexicon_passbokning.Validation;
using System.ComponentModel.DataAnnotations;

public class DurationInRangeAttribute : ValidationAttribute
{ 
    private readonly TimeSpan _minDuration = TimeSpan.FromHours(0);
    private readonly TimeSpan _maxDuration = TimeSpan.FromHours(24);

    public DurationInRangeAttribute() : base("The duration must be between 00:00 and 24:00.")
    {
    }

    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return true;
        }

        if (value is TimeSpan timeSpan)
        {
            return timeSpan >= _minDuration && timeSpan <= _maxDuration;
        }

        return false;
    }
}
