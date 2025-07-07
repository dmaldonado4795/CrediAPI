using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrediAPI.Domain.Models
{
    public class ProductoModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string Nombre { get; set; }
        [Required]
        public decimal TasaInteres { get; set; }
        [Required]
        public decimal MontoMinimo { get; set; }
        [Required]
        public decimal MontoMaximo { get; set; }
    }
}