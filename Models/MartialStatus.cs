﻿namespace Etaa.Models
{
    public class MartialStatus
    {
        [Key]
        public int MartialStatusId { get; set; }
        [Required(ErrorMessage = "Name(Ar) is Required!")]
        [Display(Name = "Name(Ar)")]
        public string NameAr { get; set; }
        public string? NameEn { get; set; }
        public bool? IsCanceled { get; set; }

        // Each state contains a number of cities, This property defines the relationship between them
        public ICollection<Family>? Families;
    }
}
