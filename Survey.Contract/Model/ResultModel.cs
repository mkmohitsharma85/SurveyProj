namespace Survey.Contract.Model
{
    public class ResultModel<T>
    {
        public T Result { get; set; }
        public BaseModel BaseModel { get; set; }
    }

    public class BaseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }

}
