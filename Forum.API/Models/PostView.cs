using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.API.Models
{
    public class PostView
    {
        public int? PostId { get; set; }

        public string Header { get; set; }

        public string Content { get; set; }

        public DateTime? Published { get; set; }

    }
}
