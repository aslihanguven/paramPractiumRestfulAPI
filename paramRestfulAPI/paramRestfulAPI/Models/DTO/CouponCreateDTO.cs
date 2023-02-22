using System.ComponentModel.DataAnnotations;

namespace paramRestfulAPI.Models.DTO
{
    public class CouponCreateDTO
    {
        [Required]
        public string Name { get; set; }

        public int Percent { get; set; }

        public bool IsUsable { get; set; }
    }
}
