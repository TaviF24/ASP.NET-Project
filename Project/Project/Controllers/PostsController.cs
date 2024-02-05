using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services.PostsService;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostsService _postsService;

        public PostsController(IPostsService postsService)
        {
            _postsService = postsService;
        }

        [HttpGet, Authorize(Roles = "User")]
        [Route("GetFirst3UsersPost")]

        public async Task<IActionResult> GetFirst3UsersPost()
        {
            return Ok(await _postsService.GetFirst3UsersPosts());
            
        }

        [HttpGet, Authorize(Roles = "User")]
        [Route("GetAllPosts")]
        public async Task<IActionResult> GetAllPosts()
        {
            return Ok(await _postsService.GetAllPosts());
        }


        [HttpPost, Authorize(Roles = "User")]
        [Route("CreatePost")]
        public async Task<IActionResult> CreatePost(string DisName_or_Id, string text)
        {
            if (await _postsService.CreatePost(DisName_or_Id,text) == true)
                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Succes", Message = "Post created successfully" });
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "Error" });
        }

        [HttpPut, Authorize(Roles = "User")]
        [Route("UpdatePost")]
        public async Task<IActionResult> UpdatePost(Guid postId, string text)
        {
            if (await _postsService.UpdatePost(postId,text) == true)
                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Succes", Message = "Post updated successfully" });
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "Error" });
        }


        [HttpDelete, Authorize(Roles = "User")]
        [Route("DeletePost")]
        public async Task<IActionResult> DeletePost(Guid postId)
        {
            if (await _postsService.DeletePost(postId) == true)
                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Succes", Message = "Post deleted successfully" });
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "Error" });
        }
    }
}
