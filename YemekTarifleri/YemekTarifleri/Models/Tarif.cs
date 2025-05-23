using System.ComponentModel.DataAnnotations;

namespace YemekTarifleri.Models
{
    public class Tarif
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Baslik { get; set; }

        [Required]
        public string Icerik { get; set; }

        public string ResimYolu { get; set; }

        [DataType(DataType.Date)]
        public DateTime EklenmeTarihi { get; set; } = DateTime.Now;
    }
}

