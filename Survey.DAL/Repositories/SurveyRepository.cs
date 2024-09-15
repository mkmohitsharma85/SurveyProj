using Microsoft.EntityFrameworkCore;
using Survey.DAL.DBModel;
using Survey.DAL.IRepositories;

namespace Survey.DAL.Repositories
{
    public class SurveyRepository : ISurveyRepository
    {
        private readonly ApplicationDBContext _context;

        public SurveyRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DBModel.Survey>> GetAllAsync()
        {
            return await _context.Surveys.Include(s => s.Questions).ToListAsync();
        }

        public async Task<DBModel.Survey> GetByIdAsync(int id)
        {
            return await _context.Surveys.Include(s => s.Questions).FirstOrDefaultAsync(s => s.Id == id) ?? new DBModel.Survey();
        }

        public async Task AddAsync(DBModel.Survey survey)
        {
            _context.Surveys.Add(survey);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(DBModel.Survey survey)
        {
            _context.Entry(survey).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var survey = await _context.Surveys.FindAsync(id);
            if (survey != null)
            {
                _context.Surveys.Remove(survey);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddResponseAsync(SurveyResponses response)
        {
            _context.SurveyResponses.Add(response);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SurveyResponses>> GetResponsesByUserIdAsync(string userId)
        {
            return await _context.SurveyResponses
                .Where(r => r.UserId == userId)
                .Include(r => r.QuestionResponses)
                .ToListAsync();
        }

        public async Task AssignSurveyAsync(SurveyAssignment assignmentInput)
        {
            try
            {
                _context.SurveyAssignments.Add(assignmentInput);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var aa = ex;
                throw;
            }
        }

        public async Task<IEnumerable<SurveyAssignment>> GetAssignmentsByUserIdAsync(string userId)
        {
            return await _context.SurveyAssignments
                .Where(sa => sa.UserId == userId)
                .Select(sa => new SurveyAssignment
                {
                    SurveyId = sa.SurveyId,
                    UserId = sa.UserId,
                    AssignedDate = sa.AssignedDate,
                    Status = sa.Status
                }).ToListAsync();
        }

        public async Task<IEnumerable<SurveyAssignment>> GetAssignmentsBySurveyIdAsync(int surveyId)
        {
            return await _context.SurveyAssignments
                .Where(sa => sa.SurveyId == surveyId)
                .Select(sa => new SurveyAssignment
                {
                    SurveyId = sa.SurveyId,
                    UserId = sa.UserId,
                    AssignedDate = sa.AssignedDate,
                    Status = sa.Status
                }).ToListAsync();
        }
    }
}

