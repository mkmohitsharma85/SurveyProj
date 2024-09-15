namespace Survey.DAL.DBModel
{
    public class Survey
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public List<Question> Questions { get; set; }
        public List<SurveyAssignment> Assignments { get; set; }
    }
}
