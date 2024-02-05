using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services.CommentsService;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {

        private readonly ICommentsService _commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }


        [HttpGet]
        [Route("GetAllComments")]
        public IActionResult GetAllComments()
        {
            return Ok(_commentsService.GetAllComm());
        }


        [HttpPost]
        [Route("CreateComment")]
        public async Task<IActionResult> CreateComment(string DisName_or_Id, Guid postId, string text)
        {
            if (await _commentsService.CreateComment(DisName_or_Id, postId, text) == true)
                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Succes", Message = "Post created successfully" });
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "Error" });
        }

        [HttpPut]
        [Route("UpdateComment")]
        public async Task<IActionResult> UpdateComment(Guid commentId, string text)
        {
            if (await _commentsService.UpdateComm(commentId, text) == true)
                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Succes", Message = "Post updated successfully" });
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "Error" });
        }


        [HttpDelete]
        [Route("DeleteComment")]
        public async Task<IActionResult> DeleteComment(Guid commId)
        {
            if (await _commentsService.DeleteComm(commId) == true)
                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Succes", Message = "Post deleted successfully" });
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "Error" });
        }

    }
}
