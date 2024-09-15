namespace Survey.Contract.Model
{
    public class QuestionResponseDTO
    {
        public int Id { get; set; }
        public int SurveyResponseId { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
    }
}
