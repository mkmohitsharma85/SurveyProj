using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Survey.BLL.IService;
using Survey.Contract.Model;
using System.Net;
using System.Security.Claims;

namespace DemoTestProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveysController : ControllerBase
    {
        private readonly ISurveyService _service;
        private readonly UserManager<IdentityUser> _userManager;
        public SurveysController(ISurveyService service, UserManager<IdentityUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SurveyDTO>>> GetSurveys()
        {
            try
            {

                var surveys = await _service.GetAllAsync();

                var result = new ResultModel<object>
                {
                    Result = surveys,
                    BaseModel = new BaseModel
                    {
                        Status = HttpStatusCode.OK.ToString(),
                        Message = "Success"
                    }
                };
                return Ok(surveys);

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

        [HttpGet("{id}")]
        public async Task<ActionResult<SurveyDTO>> GetSurvey(int id)
        {
            try
            {

                var survey = await _service.GetByIdAsync(id);
                if (survey == null)
                {
                    var result = new ResultModel<object>
                    {
                        Result = null,
                        BaseModel = new BaseModel
                        {
                            Status = HttpStatusCode.NotFound.ToString(),
                            Message = "Data Not Found"
                        }
                    };
                }
                return Ok(survey);

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

        [HttpPost]
        public async Task<ActionResult> PostSurvey(SurveyDTO SurveyDTO)
        {
            await _service.AddAsync(SurveyDTO);
            return CreatedAtAction(nameof(GetSurvey), new { id = SurveyDTO.Id }, SurveyDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSurvey(int id, SurveyDTO SurveyDTO)
        {
            if (id != SurveyDTO.Id)
            {
                return BadRequest();
            }
            await _service.UpdateAsync(SurveyDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSurvey(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("Submit")]
        [Authorize(Roles = "Admin")] // Only allow Admin role
        public async Task<IActionResult> SubmitSurveyResponse([FromBody] SurveyResponsesDTO surveyResponseDTO)
        {
            if (surveyResponseDTO == null)
            {
                return BadRequest(new ResultModel<object>
                {
                    Result = null,
                    BaseModel = new BaseModel
                    {
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Survey response data is required."
                    }
                });
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Unauthorized(new ResultModel<object>
                    {
                        Result = null,
                        BaseModel = new BaseModel
                        {
                            Status = HttpStatusCode.Unauthorized.ToString(),
                            Message = "User not authenticated."
                        }
                    });
                }

                surveyResponseDTO.UserId = userId;

                await _service.AddResponseAsync(surveyResponseDTO);

                return Ok(new ResultModel<SurveyResponsesDTO>
                {
                    Result = surveyResponseDTO,
                    BaseModel = new BaseModel
                    {
                        Status = HttpStatusCode.OK.ToString(),
                        Message = "Survey response submitted successfully."
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultModel<object>
                {
                    Result = null,
                    BaseModel = new BaseModel
                    {
                        Status = HttpStatusCode.InternalServerError.ToString(),
                        Message = $"Error: {ex.Message}"
                    }
                });
            }
        }

        [HttpGet("Get/{userID}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSurveyResponses(string userID)
        {
            try
            {
                var responses = await _service.GetResponsesByUserIdAsync(userID);

                return Ok(new ResultModel<IEnumerable<SurveyResponsesDTO>>
                {
                    Result = responses,
                    BaseModel = new BaseModel
                    {
                        Status = HttpStatusCode.OK.ToString(),
                        Message = "Survey responses retrieved successfully."
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultModel<object>
                {
                    Result = null,
                    BaseModel = new BaseModel
                    {
                        Status = HttpStatusCode.InternalServerError.ToString(),
                        Message = $"Error: {ex.Message}"
                    }
                });
            }
        }

        [HttpPost("Assign")]
        [Authorize(Roles = "Admin")] // Only allow Admin role
        public async Task<IActionResult> AssignSurvey([FromBody] SurveyAssignmentDTO assignmentInput)
        {
            if (assignmentInput == null)
            {
                return BadRequest(new ResultModel<object>
                {
                    Result = null,
                    BaseModel = new BaseModel
                    {
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Assignment data is required."
                    }
                });
            }

            try
            {
                var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByEmailAsync(userName);
                assignmentInput.UserId = user.Id;
                await _service.AssignSurveyAsync(assignmentInput);

                return Ok(new ResultModel<SurveyAssignmentDTO>
                {
                    Result = new SurveyAssignmentDTO
                    {
                        SurveyId = assignmentInput.SurveyId,
                        UserId = assignmentInput.UserId,
                        AssignedDate = DateTime.UtcNow,
                        Status = assignmentInput.Status
                    },
                    BaseModel = new BaseModel
                    {
                        Status = HttpStatusCode.OK.ToString(),
                        Message = "Survey assigned successfully."
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultModel<object>
                {
                    Result = null,
                    BaseModel = new BaseModel
                    {
                        Status = HttpStatusCode.InternalServerError.ToString(),
                        Message = $"Error: {ex.Message}"
                    }
                });
            }
        }

        // GET: api/SurveyAssignment/GetByUser/{userId}
        [HttpGet("GetByUser/{userId}")]
        [Authorize] // Any authenticated user can access
        public async Task<IActionResult> GetAssignmentsByUserId(string userId)
        {
            try
            {
                var assignments = await _service.GetAssignmentsByUserIdAsync(userId);

                return Ok(new ResultModel<IEnumerable<SurveyAssignmentDTO>>
                {
                    Result = assignments,
                    BaseModel = new BaseModel
                    {
                        Status = HttpStatusCode.OK.ToString(),
                        Message = "Assignments retrieved successfully."
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultModel<object>
                {
                    Result = null,
                    BaseModel = new BaseModel
                    {
                        Status = HttpStatusCode.InternalServerError.ToString(),
                        Message = $"Error: {ex.Message}"
                    }
                });
            }
        }

        // GET: api/SurveyAssignment/GetBySurvey/{surveyId}
        [HttpGet("GetBySurvey/{surveyId}")]
        [Authorize] // Any authenticated user can access
        public async Task<IActionResult> GetAssignmentsBySurveyId(int surveyId)
        {
            try
            {
                var assignments = await _service.GetAssignmentsBySurveyIdAsync(surveyId);

                return Ok(new ResultModel<IEnumerable<SurveyAssignmentDTO>>
                {
                    Result = assignments,
                    BaseModel = new BaseModel
                    {
                        Status = HttpStatusCode.OK.ToString(),
                        Message = "Assignments retrieved successfully."
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResultModel<object>
                {
                    Result = null,
                    BaseModel = new BaseModel
                    {
                        Status = HttpStatusCode.InternalServerError.ToString(),
                        Message = $"Error: {ex.Message}"
                    }
                });
            }
        }

    }
}
