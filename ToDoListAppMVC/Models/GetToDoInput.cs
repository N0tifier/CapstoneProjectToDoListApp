namespace ToDoListAppMVC.Models
{
    public class GetToDoInput
    {
        public bool ImportantOnly { get; set; }
        public bool TodayOnly { get; set; }
    }
}