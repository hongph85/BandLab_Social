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
        private SocialService _service;

        public PostsController(SocialService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<Post> Get()
        {
            return _service.GetPosts();
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public IActionResult Create(
            string id,
            string caption,
            [AllowedExtensions(new[] {".jpg", ".png", ".jpeg", ".bmp" })]
            [MaxFileSize(100* 1024 * 1024)]
            IFormFile file,
            string userId)
        {
            _service.AddPost(id, caption, file, new Guid(userId));
            return Ok();
        }
    }
}
