using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostWebAPI.Controllers
{
    public class PostRepository
    {
        CosmosDBContext context;
        CosmosClient client;
        private static readonly string CosmosDatabaseId = "BandlabDB";
        private static readonly string containerId = "Posts";

        public PostRepository(CosmosDBContext context, CosmosClient client)
        {
            this.context = context;
            this.client = client;
        }

        public IEnumerable<Post> GetAllPosts()
        {
            return context.Posts.ToList();
        }

        private async Task<Container> GetOrCreateContainerAsync(Database database, string containerId)
        {
            ContainerProperties containerProperties = new ContainerProperties(id: containerId, partitionKeyPath: "/PostId");

            return await database.CreateContainerIfNotExistsAsync(
                containerProperties: containerProperties,
                throughput: 400);
        }

        public async Task<PostResponse> GetPosts(int pageSize, string continuationToken)
        {
            var postResponse = new PostResponse();

            var cosmosDatabase = await client.CreateDatabaseIfNotExistsAsync(CosmosDatabaseId);
            var _container = await GetOrCreateContainerAsync(cosmosDatabase, containerId);
            var posts = new List<Post>();
            var queryDef = new QueryDefinition("select * from Post p Order by p.TotalComments Desc");
            string token = null;
            if (!string.IsNullOrWhiteSpace(continuationToken))
                token = continuationToken;

            using FeedIterator<Post> resultSet = _container.GetItemQueryIterator<Post>(queryDefinition: queryDef, continuationToken: token, new QueryRequestOptions { MaxItemCount = pageSize });
            {
                var items = await resultSet.ReadNextAsync();
                foreach (Post item in items)
                {
                    posts.Add(item);
                }

                postResponse.Posts = posts.OrderByDescending(x => x.RecentComments?.Count);
                postResponse.ContinuationToken = items.ContinuationToken;
            }

            return postResponse;
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
                context.Comments.Add(new Comment() { 
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
            var item = context.Comments.Where(x => x.PostId == postId && x.Comments.Any(y => y.Id == id)).FirstOrDefault();
            context.Comments.Remove(item);
        }

        public void Commit()
        {
            context.SaveChanges();
        }

    }
}