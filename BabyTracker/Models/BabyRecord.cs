using SQLite;

namespace BabyTracker.Models
{
    public enum RecordType
    {
        Wakeup,
        Feeding
    }

    public class BabyRecord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public RecordType Type { get; set; }
        
        public DateTime Timestamp { get; set; }
        
        public string? Notes { get; set; }
    }
}
