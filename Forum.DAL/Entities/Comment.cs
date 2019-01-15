using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Forum.DAL.Entities
{
    [Table("Comments")]
    public class Comment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }

        [Required]
        public int PostId { get; set; }

        public Post Post { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        [Required, MaxLength(1500)]
        public string Content { get; set; }

        [Required]
        public DateTime Published { get; set; }
    }
}
