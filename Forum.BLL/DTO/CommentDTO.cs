using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.BLL.DTO
{
    public class CommentDTO
    {
        public int? CommentId { get; set; }

        public int? UserId { get; set; }

        public int? PostId { get; set; }

        public string Content { get; set; }

        public DateTime? Published { get; set; }
    }
}
