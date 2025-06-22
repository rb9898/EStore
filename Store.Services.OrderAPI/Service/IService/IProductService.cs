
using Store.Services.OrderAPI.Models.Dto;

namespace Store.Services.OrderAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
