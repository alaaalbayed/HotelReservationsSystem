namespace Domain.DTO_s
{
    public class Log
    {
        public int Id { get; set; }
        public string Level { get; set; }
        public string Class { get; set; }
        public string Method { get; set; }
        public string Exception { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
