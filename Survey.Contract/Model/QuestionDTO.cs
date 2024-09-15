namespace Survey.Contract.Model
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; } // Multiple Choice, Short Answer
        public List<ChoiceDTO> Choices { get; set; }
    }
}
