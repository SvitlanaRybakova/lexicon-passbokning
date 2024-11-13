﻿using Microsoft.AspNetCore.Identity;

namespace lexicon_passbokning.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
