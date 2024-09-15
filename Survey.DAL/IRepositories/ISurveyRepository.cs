using Survey.DAL.DBModel;

namespace Survey.DAL.IRepositories
{
    public interface ISurveyRepository
    {
        Task<IEnumerable<DBModel.Survey>> GetAllAsync();
        Task<DBModel.Survey> GetByIdAsync(int id);
        Task AddAsync(DBModel.Survey survey);
        Task UpdateAsync(DBModel.Survey survey);
        Task DeleteAsync(int id);
        Task AddResponseAsync(SurveyResponses response);
        Task<IEnumerable<SurveyResponses>> GetResponsesByUserIdAsync(string userId);

        Task AssignSurveyAsync(SurveyAssignment assignmentInput);
        Task<IEnumerable<SurveyAssignment>> GetAssignmentsByUserIdAsync(string userId);
        Task<IEnumerable<SurveyAssignment>> GetAssignmentsBySurveyIdAsync(int surveyId);
    }
}
