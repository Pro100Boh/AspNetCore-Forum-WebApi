using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.BLL.DTO
{
    public class PostDTO
    {
        public int? PostId { get; set; }

        public string Header { get; set; }

        public string Content { get; set; }

        public DateTime? Published { get; set; }

    }
}
