using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Project.Models;
using Project.Models.Authentification.Login;
using Project.Models.Authentification.SignUp;
using ProjectService.Models;
using ProjectService.Services;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthentificationController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IEmailService emailService, IConfiguration configuration, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _configuration = configuration;
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
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser, string role)
        {
            //Check user exists

            var userExists = await _userManager.FindByEmailAsync(registerUser.Email);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Error", Message = "User already exists!" });
            }



            //Add the user in the database

            IdentityUser user = new()
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.Username,
                TwoFactorEnabled = true
            };

            if (await _roleManager.RoleExistsAsync(role))
            {
                var result = await _userManager.CreateAsync(user, registerUser.Password);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new Response { Status = "Error", Message = "Failed to create user!" });

                }

                //Add role to the user

                await _userManager.AddToRoleAsync(user, role);


                //Add token to verify the email

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentification", new { token, email = user.Email }, Request.Scheme);
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
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {

            //Checking the user and the password

            //var user = await _userManager.FindByNameAsync(loginModel.Username_or_Email);

            var user_name = await _userManager.FindByNameAsync(loginModel.Username_or_Email);
            var user_email = await _userManager.FindByEmailAsync(loginModel.Username_or_Email);
            IdentityUser user;
            if (user_name != null || user_email != null)
            {
                if (user_name != null)
                    user = user_name;
                else
                    user = user_email;
                /*
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
                */
                //if (user != null)
                //{

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

                if (await _userManager.CheckPasswordAsync(user, loginModel.Password))
                {
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

                    var jwtToken = GetToken(authClaims);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        expiration = jwtToken.ValidTo
                    });
                }
            }
            /*
            if ((user_name != null || user_email!=null) && await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                //we add roles to the list
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach(var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var jwtToken = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    expiration = jwtToken.ValidTo
                });

            }
            */
            return Unauthorized();




            //Claimlist creation




        }


        [HttpPost]
        [Route("login-2FA")]
        public async Task<IActionResult> LoginWithOTP(string code, string Username_or_Email)
        {
            var signIn = await _signInManager.TwoFactorSignInAsync("Email", code, false, false);
            if (signIn.Succeeded)
            {
                var user_name = await _userManager.FindByNameAsync(Username_or_Email);
                var user_email = await _userManager.FindByEmailAsync(Username_or_Email);
                IdentityUser user;

                //var user = await _userManager.FindByNameAsync(Username_Email);

                if (user_name != null || user_email != null)
                {
                    if (user_name != null)
                        user = user_name;
                    else
                        user = user_email;
                    //if (user != null) {
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

                    var jwtToken = GetToken(authClaims);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        expiration = jwtToken.ValidTo
                    });

                }
            }
            return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Error", Message = "Invalid code!" });
        }



        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

    }
}




/*
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Models.Authentification.SignUp;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {


        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;


        public AuthenticationController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }




        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser, string role)
        {
            //Check user exists

            var userExists = await _userManager.FindByEmailAsync(registerUser.Email);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Error", Message = "User already exists!" });
            }



            //Add the user in the database

            IdentityUser user = new()
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.Username,
                TwoFactorEnabled = true
            };

            if (await _roleManager.RoleExistsAsync(role))
            {
                var result = await _userManager.CreateAsync(user, registerUser.Password);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new Response { Status = "Error", Message = "Failed to create user!" });

                }

                //Add role to the user

                await _userManager.AddToRoleAsync(user, role);


                //Add token to verify the email

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentification", new { token, email = user.Email }, Request.Scheme);
                // cand persoana da click pe link, se apeleaza functia ConfirmEmail de mai sus
                //var message = new Message(new string[] { user.Email }, "Confirmation email link", confirmationLink);
                //_emailService.sendEmail(message);

                return StatusCode(StatusCodes.Status201Created,
                        new Response { Status = "Success", Message = $"User created & Email sent to {user.Email} successfully" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new Response { Status = "Error", Message = "This role does not exist!" });
            }
        }




    }
}

*/
