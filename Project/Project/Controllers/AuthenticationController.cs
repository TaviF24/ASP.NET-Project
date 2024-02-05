using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Models.Authentication.SignUp;
using Project.Models.Authentification.Login;
using Project.Models.Authentification.SignUp;
using Project.Models.Email;
using Project.Services.EmailService;
using Project.Services.TokenService;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        public AuthenticationController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IEmailService emailService, ITokenService tokenService, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }


        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Succes", Message = "Email verified successfully" });
                }

            }
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Inexistent User" });

        }


        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserModel registerUserModel, string role)
        {
            //Check user exists

            var userExists = await _userManager.FindByEmailAsync(registerUserModel.Email);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Error", Message = "User already exists!" });
            }


            //Add the user in the database

            User user = new()
            {
                Email = registerUserModel.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUserModel.Username,
                TwoFactorEnabled = true
            };

            if (await _roleManager.RoleExistsAsync(role))
            {
                var result = await _userManager.CreateAsync(user, registerUserModel.Password);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new Response { Status = "Error", Message = "Failed to create user!" });

                }

                //Add role to the user

                await _userManager.AddToRoleAsync(user, role);


                //Add token to verify the email

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token, email = user.Email }, Request.Scheme);
                // cand persoana da click pe link, se apeleaza functia ConfirmEmail de mai sus
                var message = new Message(new string[] { user.Email }, "Confirmation email link", confirmationLink);
                _emailService.sendEmail(message);

                return StatusCode(StatusCodes.Status201Created,
                        new Response { Status = "Success", Message = $"User created & Email sent to {user.Email} successfully" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new Response { Status = "Error", Message = "This role does not exist!" });
            }





        }



        [HttpDelete]
        public async Task<IActionResult> DeleteByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new Response { Status = "Error", Message = "User does not exist" });

            await _userManager.DeleteAsync(user);
            return StatusCode(StatusCodes.Status201Created,
                        new Response { Status = "Success", Message = $"User removed successfully" });
        }


        //[HttpGet]
        //public IActionResult TestEmail()
        //{
        //    var message = new Message(new string[] {"farcasioctavian@gmail.com"}, "Test", "Testing");
        //    _emailService.sendEmail(message);
        //    return StatusCode(StatusCodes.Status201Created,
        //                new Response { Status = "Success", Message = "Email sent successfully" });
        //}


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserModel loginModel)
        {

            //Checking the user and the password

            var user_name = await _userManager.FindByNameAsync(loginModel.Username_or_Email);
            var user_email = await _userManager.FindByEmailAsync(loginModel.Username_or_Email);
            User user;
            if (user_name != null || user_email != null)
            {
                if (user_name != null)
                    user = user_name;
                else
                    user = user_email;

                if(! await _userManager.CheckPasswordAsync(user, loginModel.Password) )
                    return Unauthorized("Email or password invalid.");
                if (user.TwoFactorEnabled)
                {

                    await _signInManager.SignOutAsync();
                    await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, true);

                    var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                    var message = new Message(new string[] { user.Email! }, "OTP Confirmation", token);
                    
                    _emailService.sendEmail(message);

                    return StatusCode(StatusCodes.Status200OK,
                        new Response { Status = "Succes", Message = $"We have sent an OTP to your email {user.Email}" });
                }
            }

            return Unauthorized("Email or password invalid.");
        }


        [HttpPost]
        [Route("login-2FA")]
        public async Task<IActionResult> LoginWithOTP(string code, string Username_or_Email)
        {
            var user_name = await _userManager.FindByNameAsync(Username_or_Email);
            var user_email = await _userManager.FindByEmailAsync(Username_or_Email);
            User user;
            if(user_name == null && user_email ==null)
                return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Error", Message = "User not found!" });

            var signIn = await _signInManager.TwoFactorSignInAsync("Email", code, false, false);
            if (signIn.Succeeded)
            {
                if (user_name != null)
                    user = user_name;
                else
                    user = user_email;

                var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                //we add roles to the list
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var jt = _tokenService.GetToken(authClaims);
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(jt);

                await _tokenService.AddRefreshTokenToUser(user.Id,jwtToken);

                return Ok(new
                {
                    token = jwtToken,
                    expiration = jt.ValidTo
                });             
            }

            return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Error", Message = "Invalid code!" });
        }





        [HttpPost]
        [AllowAnonymous]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([Required] string email)
        {     
            var user = await _userManager.FindByEmailAsync(email);
            if(user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var forgotPasswordLink = Url.Action(nameof(ResetPassword), "Authentication", new { token, email = user.Email }, Request.Scheme);
                var message = new Message(new string[] { user.Email! }, "Forgot password link", forgotPasswordLink);
                _emailService.sendEmail(message);
                return StatusCode(StatusCodes.Status200OK,
                        new Response { Status = "Succes", Message = $"Password changed request is sent on email {email} . Please verify your email" });

            }
            return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Error", Message = "Could not send the email, please try again!" });
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult>ResetPassword(string token, string email)
        {
            var model = new ResetPasswordModel { Token = token, Email = email };
            return Ok( new { model });
        }


        [HttpPut]
        [AllowAnonymous]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email ); 
            if (user != null)
            {
                var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
                if (!resetPassResult.Succeeded)
                {
                    foreach (var error in resetPassResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return Ok(ModelState);
                }

                return StatusCode(StatusCodes.Status200OK,
                        new Response { Status = "Succes", Message = "Password has been changed." });

            }
            return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Error", Message = "Could not send the email, please try again!" });
        }



        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([Required] string Email_or_Username)
        {
            var code = await _tokenService.Refresh(Email_or_Username);

            switch (code)
            {
                case "1":
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Error", Message = "User not found!" });
                case "2":
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Error", Message = "Token expired. Please log in again." });
                default:
                    return StatusCode(StatusCodes.Status200OK,
                        new Response { Status = "Succes", Message = code});
            }

         

        }


        /*
        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> LogoutUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            
            return Ok();
        }
        */

    }
}

