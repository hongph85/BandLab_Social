using Microsoft.AspNetCore.Http;
using PostWebAPI.Controllers;
using PostWebAPI.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace PostWebAPI.Repositories
{
    public class PostRepository : ReadingPostRepository
    {
        CosmosDBContext context;

        public PostRepository(string cosmosDatabaseId, string containerId, string endpoint, string token) : base(cosmosDatabaseId, containerId, endpoint, token)
        {
            context = new CosmosDBContext(endpoint, cosmosDatabaseId, token);
        }

        public IEnumerable<Post> GetAllPosts()
        {
            return context.Posts.ToList();
        }

        public void AddPost(Post post)
        {
            context.Posts.Add(post);
        }

        public void AddComment(long id, string comment, string author, string postId)
        {
            var currentPost = context.Posts.Where(x => x.PostId == postId).FirstOrDefault();
            var item = new CommentRow()
            {
                Id = id,
                Comment = comment,
                Author = author
            };

            if (currentPost != null)
            {
                var firstRC = currentPost.RecentComments.FirstOrDefault();
                if (firstRC != null)
                {
                    currentPost.RecentComments.Remove(currentPost.RecentComments.FirstOrDefault());
                }
                else
                {
                    currentPost.RecentComments = new List<CommentRow>();
                }

                currentPost.RecentComments.Add(item);
                currentPost.TotalComments++;
            }

            var commentsOfPost = context.Comments.Where(x => x.PostId == postId).FirstOrDefault();

            if (commentsOfPost != null)
            {
                commentsOfPost.Comments.Add(item);
            }
            else
            {
                context.Comments.Add(new Comment()
                {
                    PostId = postId,
                    Comments = new List<CommentRow>() { item }
                });
            }
        }

        public void DeletePost(string id)
        {
            var post = context.Posts.Where(x => x.PostId == id).FirstOrDefault();
            context.Posts.Remove(post);
        }

        public void DeleteComment(string postId, long id)
        {
            var commentsOfPost = context.Comments.Where(x => x.PostId == postId).FirstOrDefault();
            var commentRow = commentsOfPost.Comments.Where(y => y.Id == id).FirstOrDefault();
            commentsOfPost.Comments.Remove(commentRow);
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public override void Dispose()
        {
            base.Dispose();
            context.Dispose();
        }
    }
}

