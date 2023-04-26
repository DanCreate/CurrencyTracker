using System.ComponentModel.DataAnnotations;

namespace CurrencyTracker.Models
{
    public class Rates
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }
        [Display(Name = "Date")]
        public string FormattedDate => Date.ToShortDateString();
        public decimal USD { get; set; }
        public decimal CrVenta { get; set; }
        public decimal CrCompra { get; set; }
    }
}
