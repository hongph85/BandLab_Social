using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ILogger<PostsController> _logger;
        private SocialService _service;

        public PostsController(ILogger<PostsController> logger, SocialService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IEnumerable<Post> Get()
        {
            return _service.GetPosts();
        }

        [HttpPost]
        public IActionResult Create(IFormFile file, string caption, string id, string userId)
        {
            _service.AddPost(file, caption, id, new Guid(userId));
            return Ok();
        }
    }
}
