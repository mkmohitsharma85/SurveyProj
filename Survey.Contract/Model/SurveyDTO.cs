namespace Survey.Contract.Model
{
    public class SurveyDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public List<QuestionDTO> Questions { get; set; }
    }
}
