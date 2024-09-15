using AutoMapper;
using Survey.BLL.IService;
using Survey.Contract.Model;
using Survey.DAL.IRepositories;
using Survey.DAL.DBModel;

namespace Survey.BLL.Service
{
    public class SurveyService : ISurveyService
    {
        private readonly ISurveyRepository _repository;
        private readonly IMapper _mapper;

        public SurveyService(ISurveyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SurveyDTO>> GetAllAsync()
        {
            var surveys = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<SurveyDTO>>(surveys);
        }

        public async Task<SurveyDTO> GetByIdAsync(int id)
        {
            var survey = await _repository.GetByIdAsync(id);
            return _mapper.Map<SurveyDTO>(survey);
        }

        public async Task AddAsync(SurveyDTO SurveyDTO)
        {
            try
            {
                var survey = _mapper.Map<DAL.DBModel.Survey>(SurveyDTO);
                await _repository.AddAsync(survey);
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                throw;
            }
        }

        public async Task UpdateAsync(SurveyDTO SurveyDTO)
        {
            var survey = _mapper.Map<DAL.DBModel.Survey>(SurveyDTO);
            await _repository.UpdateAsync(survey);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task AddResponseAsync(SurveyResponsesDTO response)
        {
            var model = _mapper.Map<SurveyResponses>(response);
            await _repository.AddResponseAsync(model);
        }

        public async Task<IEnumerable<SurveyResponsesDTO>> GetResponsesByUserIdAsync(string userId)
        {
            var response = await _repository.GetResponsesByUserIdAsync(userId);

            return _mapper.Map<List<SurveyResponsesDTO>>(response);
        }

        public async Task AssignSurveyAsync(SurveyAssignmentDTO assignmentInput)
        {
            var model = _mapper.Map<SurveyAssignment>(assignmentInput);

            await _repository.AssignSurveyAsync(model); 
        }

        public async Task<IEnumerable<SurveyAssignmentDTO>> GetAssignmentsByUserIdAsync(string userId)
        {
            return _mapper.Map<List<SurveyAssignmentDTO>>(await _repository.GetAssignmentsByUserIdAsync(userId));
        }

        public async Task<IEnumerable<SurveyAssignmentDTO>> GetAssignmentsBySurveyIdAsync(int surveyId)
        {
            return _mapper.Map<List<SurveyAssignmentDTO>>(await _repository.GetAssignmentsBySurveyIdAsync(surveyId));
        }
    }
}
