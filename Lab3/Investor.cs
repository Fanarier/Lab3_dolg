using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab3
{
    public class Investor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int Id { get; set; }
        [Required]
        public int? DepositNumber { get; set; }
        [Required]
        public string? DepositName { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required]
        public decimal? DepositAmount { get; set; }
        [Required]
        public DateTime? DepositDate { get; set; }
        [Required]
        public decimal? InterestRate { get; set; }
        public decimal? TotalAmount { get; set; }
        public Percent? Percent { get; set; }
    }
}