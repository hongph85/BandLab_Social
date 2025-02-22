﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostWebAPI.Validation;
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
        public async Task<IActionResult> Get(string continuationToken, int pageSize = 1)
        {
            var result = await _service.GetPosts(pageSize, continuationToken);
            return Ok(result);
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public IActionResult Create(
            string id,
            string caption,
            [AllowedExtensions(new[] {".jpg", ".png", ".jpeg", ".bmp" })]
            [MaxFileSize(100 * 1024 * 1024)]
            IFormFile file,
            string author)
        {
            _service.AddPost(id, caption, file, author);
            return Ok();
        }
    }
}
