using paramRestfulAPI.Models;

namespace paramRestfulAPI.Data
{
    public static class Store
    {
        public static List<Coupon> couponList = new List<Coupon>
        {
            new Coupon{Id=1, Name= "200FF", Percent= 15, IsUsable= false},
            new Coupon{Id=2, Name="400FF", Percent=20,IsUsable=true }
        };
    }
}
