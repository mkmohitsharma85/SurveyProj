namespace Survey.DAL.DBModel
{
    public class Question
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; } // Multiple Choice, Short Answer
        public List<Choice> Choices { get; set; }
        public Survey Survey { get; set; }
    }
}
