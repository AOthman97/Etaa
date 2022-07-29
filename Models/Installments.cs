namespace Etaa.Models
{
    public class Installments
    {
        [Key]
        public int InstallmentsId { get; set; }
        [Required(ErrorMessage = "إسم القسط عربي مطلوب !")]
        [Display(Name = "إسم القسط عربي")]
        public string NameAr { get; set; }
        public string? NameEn { get; set; }
        public int? InstallmentNumber { get; set; }
        public bool? IsCanceled { get; set; }
    }
}
