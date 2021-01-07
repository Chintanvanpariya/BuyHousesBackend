
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class City
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
