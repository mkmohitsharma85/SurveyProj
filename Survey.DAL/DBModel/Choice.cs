namespace Survey.DAL.DBModel
{
    public class Choice
    {
        public int Id { get; set; }
        public string ChoiceText { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
