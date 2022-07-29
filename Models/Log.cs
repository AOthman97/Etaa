namespace Etaa.Models
{
    public class Log
    {
        [Key]
        public int LogId { get; set; }
        public string? Message { get; set; }
        public string? MessageTemplate { get; set; }
        public string? Level { get; set; }
        public string? Exception { get; set; }
        public string? Properties { get; set; }
        public DateTime TimeStamp { get; set; }

        // Relationship between the city and the state
        public int? ModuleId { get; set; }
        [ForeignKey("ModuleId")]
        public Modules? Modules { get; set; }
        // Relationship between the city and the state
        public int? EventTypeId { get; set; }
        [ForeignKey("EventTypeId")]
        public EventTypes? EventTypes { get; set; }
        public int UserId { get; set; }
        //[ForeignKey("UserId")]
        //public IdentityUser? IdentityUser { get; set; }
    }
}
