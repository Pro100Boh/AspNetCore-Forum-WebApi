using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.BLL.DTO
{
    public class UserDTO
    {
        public int? UserId { get; set; }

        public string Username { get; set; }

        public DateTime? RegistrationDate { get; set; }
    }
}
