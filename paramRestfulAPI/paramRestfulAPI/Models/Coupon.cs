namespace paramRestfulAPI.Models
{
    public class Coupon
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public int Percent { get; set; }

        public bool IsUsable { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Updated { get; set; }


    }
}
