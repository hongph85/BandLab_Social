using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PostWebAPI.Controllers
{
    public class PostRepository
    {
        CosmosDBContext context;

        public PostRepository(CosmosDBContext context)
        {
            this.context = context;
        }

        public IEnumerable<Post> GetAllPosts()
        {
            return context.Posts.ToList();
        }

        public void AddPost(Post post)
        {
            context.Posts.Add(post);
        }

        public void AddComment(Comment post)
        {
            context.Comments.Add(post);
        }

        public void DeletePost(string id)
        {
            var item = context.Posts.Where(x => x.PostId == id).FirstOrDefault();
            context.Posts.Remove(item);
        }        
        
        public void DeleteComment(string id)
        {
            var item = context.Comments.Where(x => x.CommentId == id).FirstOrDefault();
            context.Comments.Remove(item);
        }

        public void Commit()
        {
            context.SaveChanges();
        }

    }
}