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
        // int?
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        //public int AuthorId { get; set; }

        //public int PostStatusId { get; set; }

        [Required, MaxLength(200)]
        public string Header { get; set; }

        [Required, MaxLength(1500)]
        public string Content { get; set; }

        [Required]
        public DateTime Published { get; set; }
    }
}
