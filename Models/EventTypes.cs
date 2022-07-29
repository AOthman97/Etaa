namespace Etaa.Models
{
    public class EventTypes
    {
        [Key]
        public int EventTypeId { get; set; }
        [Required(ErrorMessage = "Name(Ar) is Required!")]
        [Display(Name = "Name(Ar)")]
        public bool IsEvent { get; set; }
        public bool IsException { get; set; }

        // Relationship between the event types and the log
        public ICollection<Log>? Logs;
    }
}
