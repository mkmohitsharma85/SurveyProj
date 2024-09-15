namespace Survey.DAL.DBModel
{
    public class SurveyResponses
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public string UserId { get; set; }
        public DateTime SubmittedDate { get; set; }
        public ICollection<QuestionResponse> QuestionResponses { get; set; }
    }
}
