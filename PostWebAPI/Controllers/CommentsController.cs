using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class CommentsController : ControllerBase
    {
        private SocialService _service;

        public CommentsController(SocialService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Create(string id, string comment, string userId, string postId)
        {
            _service.AddComment(id, comment, new Guid(userId), postId);
            return Ok();
        }

        [HttpDelete]
        public ActionResult Delete(string id)
        {
            _service.RemoveComment(id);
            return Ok();
        }
    }
}
