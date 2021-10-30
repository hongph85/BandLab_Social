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
        public IActionResult Create(long id, string comment, string author, string postId)
        {
            _service.AddComment(id, comment, author, postId);
            return Ok();
        }

        [HttpDelete]
        public ActionResult Delete(string postId, long id)
        {
            _service.RemoveComment(postId, id);
            return Ok();
        }
    }
}
