using AutoMapper;
using paramRestfulAPI.Models;
using paramRestfulAPI.Models.DTO;
using System.Security.Cryptography.X509Certificates;

namespace paramRestfulAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Coupon, CouponCreateDTO>().ReverseMap();
            CreateMap<Coupon, CouponDTO>().ReverseMap();

        }
    }
}
