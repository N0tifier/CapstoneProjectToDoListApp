namespace ToDoListAppMVC.Models
{
    public class GetToDoOutput
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Important { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public string RepeatType { get; set; }
        public string Note { get; set; }
    }

}