using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etaa.Models
{
    public class Log
    {
        [Key]
        public int LogId { get; set; }
        public string PageName { get; set; }
        public DateTime Date { get; set; }

        // Relationship between the city and the state
        public int ModuleId { get; set; }
        [ForeignKey("ModuleId")]
        public Modules? Modules { get; set; }
        // Relationship between the city and the state
        public int EventTypeId { get; set; }
        [ForeignKey("EventTypeId")]
        public EventTypes? EventTypes { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public Users? Users { get; set; }
    }
}
