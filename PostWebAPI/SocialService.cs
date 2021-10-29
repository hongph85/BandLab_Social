using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
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
        private CosmosDBContext _context;
        private IConfiguration _configuration;
        public SocialService(IConfiguration configuration)
        {
            _context = new CosmosDBContext();
            _postRepository = new PostRepository(_context);
            _configuration = configuration;

            var container = _configuration.GetValue<string>("ORIGIN_CONTAINER_NAME");
            var connectionString = _configuration.GetValue<string>("AzureWebJobsStorage");
            endPoint = _configuration.GetValue<string>("StorageEndpoint");

            _imageRepository = new ImageRepository(container, connectionString);

            for (var i = 0; i < 5; i++)
            {
                _postRepository.AddPost(new Post
                {
                    PostId = "P" + i,
                    Caption = "Hong_" + i,
                    User = new User() { Name = "User_" + i },
                }
                );
            }

            _postRepository.Commit();
            _configuration = configuration;
        }

        public IEnumerable<Post> GetPosts()
        {
            return _postRepository.GetAllPosts();
        }

        public void AddPost(string id, string caption, IFormFile file, Guid userId)
        {
            using (var stream = file.OpenReadStream())
                _imageRepository.Add(file.FileName, stream);

            _postRepository.AddPost(new Post(caption) { PostId = id, UserId = userId, ImagePath = endPoint + "/jpeg/" + file.FileName + ".jpg" });
            _postRepository.Commit();
        }

        public void RemoveComment(string id)
        {
            _postRepository.DeleteComment(id);
            _postRepository.Commit();
        }

        public void AddComment(string id, string comment, Guid userId, string postId)
        {
            _postRepository.AddComment(new Comment() { CommentId = id, Content = comment, UserId = userId, PostId = postId });
            _postRepository.Commit();
        }
    }
}
