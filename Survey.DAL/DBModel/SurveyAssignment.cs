namespace Survey.DAL.DBModel
{
    public class SurveyAssignment
    {
        public int ID { get; set; }
        public int SurveyId { get; set; }
        public string UserId { get; set; }
        public DateTime AssignedDate { get; set; }
        public string Status { get; set; } 
        public Survey Survey { get; set; }
    }

}
