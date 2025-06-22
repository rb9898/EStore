using Store.Web.Models;

namespace Store.Web.Service.IService
{
    public interface IOrderService
    {
        Task<ResponseDto?> CreateOrder(CartDto cartDto);
    }
}
