using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.API.Models
{
    public class UserView
    {
        public int? UserId { get; set; }

        public string Username { get; set; }

        public string Role { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public string Token { get; set; }
    }
}
