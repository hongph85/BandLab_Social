using Microsoft.AspNetCore.Http;
using PostWebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PostWebAPI
{
    public class SocialService
    {
        private UserRepository _userRepository;
        private PostRepository _postRepository;
        private CosmosDBContext _context;
        public SocialService()
        {
            _context = new CosmosDBContext();
            _postRepository = new PostRepository(_context);
            _userRepository = new UserRepository(_context);

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

            _userRepository.Commit();
        }

        public IEnumerable<Post> GetPosts()
        {
            return _postRepository.GetAllPosts();
        }

        public void AddPost(IFormFile file, string caption, string id, Guid userId)
        {
            _postRepository.AddPost(new Post(caption) { PostId = id, UserId = userId }, file);
            _postRepository.Commit();

        }

        public void RemoveComment(string id)
        {
            _postRepository.DeleteComment(id);
            _postRepository.Commit();
        }

        public void AddComment(string id, string comment, Guid userId, string postId)
        {
            _postRepository.AddComment(new Comment() { CommentId=id, Content = comment, UserId = userId, PostId = postId });
            _postRepository.Commit();
        }
    }
}
