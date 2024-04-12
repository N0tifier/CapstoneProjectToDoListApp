namespace ToDoListAppMVC.Models
{
    public class GenericResponse
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public List<GetToDoOutput>? Output { get; set; }
    }

}