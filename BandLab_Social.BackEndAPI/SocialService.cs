using BandLab_Social.Entities;
using BandLab_Social.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostWebAPI
{
    public class SocialService
    {
        private PostRepository _postRepository;
        private string blobStorageEndpoint;
        private ImageRepository _imageRepository;
        private IConfiguration _configuration;
        public SocialService(IConfiguration configuration)
        {
            _configuration = configuration;

            var blobStorageConnection = _configuration.GetValue<string>("ImageBlobStorage:AzureWebJobsStorage");
            blobStorageEndpoint = _configuration.GetValue<string>("ImageBlobStorage:StorageEndpoint");
            var container = _configuration.GetValue<string>("ImageBlobStorage:ORIGIN_CONTAINER_NAME");

            var cosmosEndpoint = _configuration.GetValue<string>("CosmosDB:Endpoint");
            var cosmosDatabaseId = _configuration.GetValue<string>("CosmosDB:DatabaseId");
            var cosmosSocialContainer = _configuration.GetValue<string>("CosmosDB:SocialContainer");
            var cosmosAuthToken = _configuration.GetValue<string>("CosmosDB:AuthToken");
            _postRepository = new PostRepository(cosmosDatabaseId, cosmosSocialContainer, cosmosEndpoint, cosmosAuthToken);
            _imageRepository = new ImageRepository(container, blobStorageConnection);

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

        public Task<PostPagination> GetPosts(int pageSize, string continuationToken)
        {
            return _postRepository.GetPosts(pageSize, continuationToken);
        }

        public void AddPost(string id, string caption, IFormFile file, string author)
        {
            using (var stream = file.OpenReadStream())
                _imageRepository.Add(file.FileName, stream);

            _postRepository.AddPost(new Post(caption) { PostId = id, Author = author, ImagePath = blobStorageEndpoint + "/jpeg/" + file.FileName + ".jpg" });
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
