using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Data.DTO.User;
using Data.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Azure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;
using System.Web;
using Microsoft.AspNetCore.Authorization;

namespace onlineShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public UserController(UserManager<AppUser> userManager, IMapper mapper, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Registration(RegsiterDTO registerDTO)
        {
            var response = new GenralResponse<RegsiterDTO>();

            // Check for model validation errors from data annotations
            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.Message = "Validation errors";
                response.Errors = ModelState.Values
                                             .SelectMany(v => v.Errors)
                                             .Select(e => e.ErrorMessage)
                                             .ToList();
                return BadRequest(response);
            }
            var user = mapper.Map<AppUser>(registerDTO);

            // Map the DTO to AppUser and create the user

            IdentityResult result = await userManager.CreateAsync(user, registerDTO.password);

            // Check if user creation was successful
            if (!result.Succeeded)
            {
                response.Success = false;
                response.Message = "User creation failed";
                response.Errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(response);
            }

            // the Email virfcation 

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmaionlink = Url.Action("ConfirmEmail", "User", new { userid = user.Id, token = token }, Request.Scheme);
            Console.WriteLine(confirmaionlink);

            // If everything is successful, return a success response
            response.Success = true;
            response.Message = "Successfully registered. Please check your email to confirm your account.";
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var response = new GenralResponse<LoginDTO>();
            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.Message = "Valdation falid";
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        response.Errors.Add(error.ErrorMessage);
                    }
                }
                return BadRequest(response);
            }
            var user = await userManager.FindByEmailAsync(loginDTO.email);
            bool result = await userManager.IsEmailConfirmedAsync(user);
            if (!result)
            {
                response.Success = false;
                response.Message = "Please confirm your email before logging in.";
                return Unauthorized(response);
            }
            if (user != null)
            {
                bool done = await userManager.CheckPasswordAsync(user, loginDTO.password);
                if (done)
                {


                    //-------------------------------------- Genrate Token --------------------------
                    var _claims = new List<Claim>();
                    _claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                    _claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    _claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                    // add rols => taks list and foter add this list in claims
                    //var role = await userManager.GetRolesAsync(user);
                    //foreach (var i in role)
                    //{
                    //    _claims.Add(new Claim(ClaimTypes.Role, i));

                    //}
                    // singing credintioal

                    SecurityKey _securitrykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:secret"].PadRight(48)));

                    SigningCredentials _singingcre = new SigningCredentials(_securitrykey, SecurityAlgorithms.HmacSha384);

                    // this token as a json

                    JwtSecurityToken mytoken = new JwtSecurityToken(
                        issuer: configuration["JWT:valid_issur"],
                        audience: configuration["JWT:valdid_audiance"],
                        claims: _claims,
                        // token end in the hour from now
                        expires: DateTime.Now.AddHours(1),
                        signingCredentials: _singingcre


                        );
                    return Ok(
                        new
                        {
                            // creat token as a compact =>   json=>>>>> compact
                            token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expiration = mytoken.ValidTo,
                            userId = user.Id,
                            //  role = role.FirstOrDefault()
                        }
                        );

                }
            }
            response.Success = false;
            response.Message = "Invalid login attempt";
            return Unauthorized(response);
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid request.");
            }

            // Decode the token if necessary
            token = HttpUtility.UrlDecode(token);

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Ok("Email confirmed successfully.");
            }

            return BadRequest("Email confirmation failed.");
        }

        //--------------------------------------- Resetpassword-----------------------
        [HttpPost("RequestPasswordReset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] ForgotPasswordDTO forgotPasswordDTO)
        {
            var response = new GenralResponse<string>();
            var user = await userManager.FindByEmailAsync(forgotPasswordDTO.Email);
            if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
            {
                response.Success = false;
                response.Message = "invalid Email";
                return Unauthorized(response);
            }


            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "User", new { token = token, email = user.Email }, Request.Scheme);

            // Send the reset link to the user's email (implement your email service here)
            Console.WriteLine(resetLink);  // Replace with email sending logic

            response.Success = true;
            response.Message = "Password reset link has been sent to your email";
            return Ok(response);
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            var response = new GenralResponse<string>();
            if (!ModelState.IsValid)
            {
                response.Success = false;
                response.Message = "Invalid request";
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        response.Errors.Add(error.ErrorMessage);
                    }
                }
                return BadRequest(response);
            }
            var user = await userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (user == null)
            {
                response.Success = false;
                response.Message = "invalid Email";
                return Unauthorized(response);
            }
            var decodedToken = System.Web.HttpUtility.UrlDecode(resetPasswordDTO.Token);
            var resetPassResult = await userManager.ResetPasswordAsync(user, decodedToken, resetPasswordDTO.NewPassword);
            if (!resetPassResult.Succeeded)
            {
                //response.Success = false;
                //response.Message = "Password reset failed.";

                //return BadRequest(response);
                return BadRequest(new { Message = "Password reset failed.", Errors = resetPassResult.Errors.Select(e => e.Description) });
            }
            response.Success = true;
            response.Message = "sussfuly to rest password";
            return Ok(response);
            //return Ok(new { Message = "Password has been reset successfully." });
        }
        [Authorize]
        [HttpGet("GetUserData")]
        public async Task<IActionResult> GetUserData()
        {
            var response = new GenralResponse<UserDataDTO>();
            // Get the current user's ID from the claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                response.Success = false;
                response.Message = "User is not authenticated ";
                return Unauthorized(response);
            }

            // Find the user by ID
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {

                response.Success = false;
                response.Message = "User  not found ";
                return NotFound(response);
            }

            // Map user data to the DTO
    var userData=     mapper.Map<UserDataDTO>(user);

            response.Success = true;
            response.Message = " Sucessfullu response";
            return Ok(userData);

        }



    }
}