using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostWebAPI
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
        public string Type = nameof(Post);
        public User User { get; set; }
        public Guid UserId { get; set; }
    }

    public class Comment
    {
        [Key]
        public string CommentId { get; set; }
        public string Content { get; set; }
        public string PostId { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public string Type = nameof(Comment);
    }

    public class User
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }

        //public ICollection<Post> Posts { get; set; }
    }
}
