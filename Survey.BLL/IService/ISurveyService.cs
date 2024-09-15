using Survey.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.BLL.IService
{
    public interface ISurveyService
    {
        Task<IEnumerable<SurveyDTO>> GetAllAsync();
        Task<SurveyDTO> GetByIdAsync(int id);
        Task AddAsync(SurveyDTO surveyDto);
        Task UpdateAsync(SurveyDTO surveyDto);
        Task DeleteAsync(int id);
        Task AddResponseAsync(SurveyResponsesDTO response);
        Task<IEnumerable<SurveyResponsesDTO>> GetResponsesByUserIdAsync(string userId);

        Task AssignSurveyAsync(SurveyAssignmentDTO assignmentInput);
        Task<IEnumerable<SurveyAssignmentDTO>> GetAssignmentsByUserIdAsync(string userId);
        Task<IEnumerable<SurveyAssignmentDTO>> GetAssignmentsBySurveyIdAsync(int surveyId);
    }
}
