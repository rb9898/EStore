using Store.Services.ShoppingCartAPI.Models.Dto;

namespace Store.Services.ShoppingCartAPI.Service.Iservice
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string couponCode);
    }
}
