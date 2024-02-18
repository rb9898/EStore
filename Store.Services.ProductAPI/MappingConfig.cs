using AutoMapper;
using Store.Services.ProductAPI.Models;
using Store.Services.ProductAPI.Models.Dto;

namespace Store.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Product,ProductDto>();
                config.CreateMap<ProductDto, Product>();
            });
            return mappingConfig;
        }
    }
}
