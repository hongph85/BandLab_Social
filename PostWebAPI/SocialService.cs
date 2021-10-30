using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using PostWebAPI.Controllers;
using PostWebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PostWebAPI
{
    public class SocialService
    {
        private PostRepository _postRepository;
        private string endPoint;
        private ImageRepository _imageRepository;
        private IConfiguration _configuration;
        public SocialService(IConfiguration configuration)
        {
            _configuration = configuration;

            var container = _configuration.GetValue<string>("ORIGIN_CONTAINER_NAME");
            var connectionString = _configuration.GetValue<string>("AzureWebJobsStorage");
            endPoint = _configuration.GetValue<string>("StorageEndpoint");
            _postRepository = new PostRepository("BandlabDB",  "Posts", "https://localhost:8081", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            _imageRepository = new ImageRepository(container, connectionString);

            for (var i = 0; i < 5; i++)
            {
                _postRepository.AddPost(new Post
                {
                    PostId = "P" + i,
                    Caption = "Hong_" + i,
                    Author = "User_" + i
                }
                );
            }

            _postRepository.Commit();
            _configuration = configuration;
        }

        public IEnumerable<Post> GetPosts()
        {
            return _postRepository.GetAllPosts().OrderByDescending(x => x.RecentComments?.Count);
        }

        public Task<PostResponse> GetPosts(int pageSize, string continuationToken)
        {
            return _postRepository.GetPosts(pageSize, continuationToken);
        }

        public void AddPost(string id, string caption, IFormFile file, string author)
        {
            using (var stream = file.OpenReadStream())
                _imageRepository.Add(file.FileName, stream);

            _postRepository.AddPost(new Post(caption) { PostId = id, Author = author, ImagePath = endPoint + "/jpeg/" + file.FileName + ".jpg" });
            _postRepository.Commit();
        }

        public void RemoveComment(string postId, long commentId)
        {
            _postRepository.DeleteComment(postId, commentId);
            _postRepository.Commit();
        }

        public void AddComment(long id, string comment, string author, string postId)
        {
            _postRepository.AddComment(id, comment, author, postId);
            _postRepository.Commit();
        }
    }
}
