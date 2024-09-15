namespace Survey.Contract.Model
{
    public class SurveyResponsesDTO
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public string UserId { get; set; }
        public DateTime SubmittedDate { get; set; }
        public List<QuestionResponseDTO> QuestionResponses { get; set; }
    }
}
