namespace TodoApi.Models
{
    public class TodoDto
    {

        public string id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? CompletedTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool Completed { get; set; }


    }
}
