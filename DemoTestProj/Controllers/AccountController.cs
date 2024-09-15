using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Survey.BLL.IService;
using Survey.Contract.Model;
using Survey.Contract.Model.Request;
using System.Net;

namespace DemoTestProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<IdentityUser> _userManager;
        
        public AccountController(ITokenService tokenService, UserManager<IdentityUser> userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestModel login)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(login.UserName);

                if (user == null)
                {
                    var notFound = new ResultModel<object>
                    {
                        Result = null,
                        BaseModel = new BaseModel
                        {
                            Status = HttpStatusCode.NotFound.ToString(),
                            Message = "User Not Found"
                        }
                    };
                    return NotFound(notFound);
                }

                if (!await _userManager.CheckPasswordAsync(user, login.Password))
                {
                    var res = new ResultModel<object>
                    {
                        Result = null,
                        BaseModel = new BaseModel
                        {
                            Status = HttpStatusCode.BadRequest.ToString(),
                            Message = "User Id and Password not correct"
                        }
                    };

                    return BadRequest(res);
                }

                var token = await _tokenService.GenerateToken(user);

                var result = new ResultModel<object>
                {
                    Result = token,
                    BaseModel = new BaseModel
                    {
                        Status = HttpStatusCode.OK.ToString(),
                        Message = "Success"
                    }
                };


                return Ok(result);

            }
            catch (Exception ex)
            {
                var result = new ResultModel<object>
                {
                    Result = null,
                    BaseModel = new BaseModel
                    {
                        Status = HttpStatusCode.InternalServerError.ToString(),
                        Message = ex.Message
                    }
                };

                return BadRequest(result);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreationRequestModel userModel)
        {
            if (!ModelState.IsValid)
            {
                var res = new ResultModel<object>
                {
                    Result = null,
                    BaseModel = new BaseModel
                    {
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Invalid user data"
                    }
                };
                return BadRequest(res);
            }

            var user = new IdentityUser
            {
                UserName = userModel.UserName,
                Email = userModel.Email
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            if (result.Succeeded)
            {
                if (userModel.Roles != null && userModel.Roles.Any())
                {
                    foreach (var role in userModel.Roles)
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }
                }
                var successResult = new ResultModel<object>
                {
                    Result = null,
                    BaseModel = new BaseModel
                    {
                        Status = HttpStatusCode.Created.ToString(),
                        Message = "User created successfully"
                    }
                };
                return CreatedAtAction(nameof(Register), successResult);
            }
            else
            {
                var errorResult = new ResultModel<object>
                {
                    Result = null,
                    BaseModel = new BaseModel
                    {
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = string.Join(", ", result.Errors.Select(e => e.Description))
                    }
                };
                return BadRequest(errorResult);
            }
        }
    }
}
