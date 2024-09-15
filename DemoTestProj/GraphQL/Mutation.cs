using Survey.BLL.IService;
using Survey.Contract.Model;
using System.Security.Claims;

namespace DemoTestProj.GraphQL
{
    public class Mutation
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Mutation(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<SurveyDTO> AddSurvey(
        [Service] ISurveyService surveyService,
        string title,
        string description,
        DateTime createdDate,
        DateTime expiryDate,
        List<QuestionDTO> questions
    )
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));

            var survey = new SurveyDTO
            {
                Title = title,
                Description = description,
                CreatedDate = createdDate,
                ExpiryDate = expiryDate,
                Questions = questions?.Select(q => new QuestionDTO
                {
                    QuestionText = q.QuestionText,
                    QuestionType = q.QuestionType,
                    Choices = q.Choices?.Select(c => new ChoiceDTO
                    {
                        ChoiceText = c.ChoiceText,
                        QuestionId = c.QuestionId
                    }).ToList() ?? new List<ChoiceDTO>()
                }).ToList() ?? new List<QuestionDTO>()
            };

            await surveyService.AddAsync(survey);
            return survey;
        }

        public async Task<SurveyDTO> UpdateSurvey(
            [Service] ISurveyService surveyService,
            int id,
            string title,
            string description,
            DateTime createdDate,
            DateTime expiryDate,
            List<QuestionDTO> questions
        )
        {
            var survey = new SurveyDTO
            {
                Id = id,
                Title = title,
                Description = description,
                CreatedDate = createdDate,
                ExpiryDate = expiryDate,
                Questions = questions ?? new List<QuestionDTO>()
            };

            await surveyService.UpdateAsync(survey);
            return survey;
        }

        public async Task<bool> DeleteSurvey([Service] ISurveyService surveyService, int id)
        {
            await surveyService.DeleteAsync(id);
            return true;
        }

        public async Task<SurveyResponsesDTO> SubmitSurveyResponse(int surveyId, List<QuestionResponseDTO> questionResponses, [Service] ISurveyService responseService)
        {
            var isAdmin = _httpContextAccessor.HttpContext.User.IsInRole("Admin");

            if (!isAdmin)
            {
                throw new UnauthorizedAccessException("User does not have permission to submit survey responses");
            }
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = new SurveyResponsesDTO
            {
                SurveyId = surveyId,
                UserId = userId,
                SubmittedDate = DateTime.UtcNow,
                QuestionResponses = questionResponses.Select(qr => new QuestionResponseDTO
                {
                    QuestionId = qr.QuestionId,
                    Answer = qr.Answer
                }).ToList()
            };

            await responseService.AddResponseAsync(response);
            return new SurveyResponsesDTO
            {
                Id = response.Id,
                SurveyId = response.SurveyId,
                UserId = response.UserId,
                SubmittedDate = response.SubmittedDate,
                QuestionResponses = response.QuestionResponses.Select(qr => new QuestionResponseDTO
                {
                    QuestionId = qr.QuestionId,
                    Answer = qr.Answer
                }).ToList()
            };
        }

        public async Task<SurveyAssignmentDTO> AssignSurvey(
       [Service] ISurveyService assignmentService,
       SurveyAssignmentDTO input)
        {
            // Extract user ID from JWT token
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if user is an admin
            var isAdmin = _httpContextAccessor.HttpContext.User.IsInRole("Admin");
            if (!isAdmin)
            {
                throw new UnauthorizedAccessException("User does not have permission to assign surveys.");
            }

            var assignment = new SurveyAssignmentDTO
            {
                SurveyId = input.SurveyId,
                UserId = userId, // Set userId to the ID from JWT token
                AssignedDate = input.AssignedDate,
                Status = input.Status
            };

            await assignmentService.AssignSurveyAsync(assignment);

            return new SurveyAssignmentDTO
            {
                SurveyId = assignment.SurveyId,
                UserId = assignment.UserId,
                AssignedDate = assignment.AssignedDate,
                Status = assignment.Status
            };
        }
    }

}
