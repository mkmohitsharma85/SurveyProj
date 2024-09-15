using Survey.BLL.IService;
using Survey.Contract.Model;

namespace DemoTestProj.GraphQL
{
    public class Query
    {
        public async Task<IEnumerable<SurveyDTO>> GetSurveys([Service] ISurveyService surveyService)
        {
            return await surveyService.GetAllAsync();
        }

        public async Task<SurveyDTO> GetSurvey(int id, [Service] ISurveyService surveyService)
        {
            return await surveyService.GetByIdAsync(id);
        }

        public async Task<IEnumerable<SurveyResponsesDTO>> GetResponsesByUserId(string userId, [Service] ISurveyService responseService)
        {
            var responses = await responseService.GetResponsesByUserIdAsync(userId);
            return responses.Select(r => new SurveyResponsesDTO
            {
                Id = r.Id,
                SurveyId = r.SurveyId,
                UserId = r.UserId,
                SubmittedDate = r.SubmittedDate,
                QuestionResponses = r.QuestionResponses.Select(qr => new QuestionResponseDTO
                {
                    QuestionId = qr.QuestionId,
                    Answer = qr.Answer
                }).ToList()
            });
        }

        public async Task<IEnumerable<SurveyAssignmentDTO>> GetAssignmentsByUserId(
        [Service] ISurveyService assignmentService,
        string userId)
        {
            var assignments = await assignmentService.GetAssignmentsByUserIdAsync(userId);
            return assignments.Select(a => new SurveyAssignmentDTO
            {
                SurveyId = a.SurveyId,
                UserId = a.UserId,
                AssignedDate = a.AssignedDate,
                Status = a.Status
            });
        }

        public async Task<IEnumerable<SurveyAssignmentDTO>> GetAssignmentsBySurveyId(
            [Service] ISurveyService assignmentService,
            int surveyId)
        {
            var assignments = await assignmentService.GetAssignmentsBySurveyIdAsync(surveyId);
            return assignments.Select(a => new SurveyAssignmentDTO
            {
                SurveyId = a.SurveyId,
                UserId = a.UserId,
                AssignedDate = a.AssignedDate,
                Status = a.Status
            });
        }
    }
}

