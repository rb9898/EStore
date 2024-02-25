using Store.Services.ShoppingCartAPI.Models.Dto;

namespace Store.Services.ShoppingCartAPI.Service.Iservice
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
