using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Forum.DAL.Entities
{
    [Table("Posts")]
    public class Post
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        [Required, MaxLength(200)]
        public string Header { get; set; }

        [Required, MaxLength(1500)]
        public string Content { get; set; }

        [Required]
        public DateTime Published { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
