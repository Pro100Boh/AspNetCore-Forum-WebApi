using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Forum.DAL.Entities
{
    public class Comment
    {
        // int?
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required, MaxLength(1500)]
        public string Content { get; set; }

        [Required]
        public DateTime Published { get; set; }
    }
}
