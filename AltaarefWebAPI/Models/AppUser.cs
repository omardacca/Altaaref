using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AltaarefWebAPI.Models
{
    public class AppUser : IdentityUser 
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public DateTime DOB { get; set; }
        public string ProfilePicBlobUrl { get; set; }
    }
}
