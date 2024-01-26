using AutoMapper;
using Store.Services.CouponAPI.Models;
using Store.Services.CouponAPI.Models.Dto;

namespace Store.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Coupon,CouponDto>();
                config.CreateMap<CouponDto, Coupon>();
            });
            return mappingConfig;
        }
    }
}
