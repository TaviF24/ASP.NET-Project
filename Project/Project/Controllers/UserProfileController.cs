using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services.UserProfileService;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet]
        [Route("GetUserPosts"), Authorize(Roles = "User")]

        public async Task<IActionResult> GetUserPosts(string DisplayedUsername)
        {
            return Ok(await _userProfileService.GetUserPosts(DisplayedUsername));
        }

        [HttpGet]
        [Route("GetUserComments"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserComments(string DisplayedUsername)
        {
            return Ok(await _userProfileService.GetUserComments(DisplayedUsername));
        }


        [HttpPost]
        [Route("CreateProfile"), Authorize(Roles = "User")]
        public async Task<IActionResult> CreateUserProfile(string Email, string DisplayedUserName, string? FirstName, string? LastName)
        {
            if (await _userProfileService.CreateProfile(Email, DisplayedUserName, FirstName, LastName) == true)
                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Succes", Message = "Profile created successfully" });
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "Error" });
        }

        [HttpPut]
        [Route("UpdateProfile"), Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateProfile(string DisplayedUserName_or_Id, string newDisplayedUserName, string newFirstName, string newLastName)
        {
            if (await _userProfileService.UpdateProfile(DisplayedUserName_or_Id, newDisplayedUserName, newFirstName, newLastName) == true)
                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Succes", Message = "Profile updated successfully" });
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "Error" });
        }


        [HttpDelete]
        [Route("DeleteProfile"), Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteProfile(string DisplayedUserName_or_Id)
        {
            if (await _userProfileService.DeleteProfile(DisplayedUserName_or_Id) == true)
                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Succes", Message = "Profile updated successfully" });
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "Error" });
        }
    }
}
