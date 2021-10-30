using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BandLab_Social.Entities
{
    public class Post
    {
        public Post()
        {
        }

        public Post(string caption)
        {
            Caption = caption;
        }

        [Key]
        public string PostId { get; set; }
        public string ImagePath { get; set; }
        public string Caption { get; set; }

        public ICollection<CommentRow> RecentComments { get; set; } = new List<CommentRow>();
        public int TotalComments { get; set; }

        public string Author { get; set; }
    }

    public class Comment
    {
        [Key]
        public string PostId { get; set; }

        public ICollection<CommentRow> Comments { get; set; } = new List<CommentRow>();
    }

    public class CommentRow
    {
        public long Id { get; set; }
        public string Author { get; set; }
        public string Comment { get; set; }
    }
}
