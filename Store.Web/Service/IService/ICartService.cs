using Store.Web.Models;

namespace Store.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDto> GetCartByUserId(string userId);
        Task<ResponseDto> UpsertCart(CartDto cartDto);
        Task<ResponseDto> RemoveFromCartAsync(int cartDetailsId);
        Task<ResponseDto> ApplyCouponAsync(CartDto cartDto);
    }
}
