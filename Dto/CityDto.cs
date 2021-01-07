
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dto
{
    public class CityDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Name is a mandatory field ")]
        [StringLength(20,MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
