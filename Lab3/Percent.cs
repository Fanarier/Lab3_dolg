using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab3
{
    public class Percent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int Id { get; set; }
        [Required]
        public int? DepositNumber { get; set; }
        [Required]
        public string? DepositName { get; set; }
        [Required]
        public decimal? InterestRate { get; set; }
        List<Investor> Investors { get; set; } = new();
    }
}