namespace TaskManagerAPI.DTOs
{
    public class HistoryDTO
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string Description { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}