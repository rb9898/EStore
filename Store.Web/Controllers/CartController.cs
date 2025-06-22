using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Store.Web.Models;
using Store.Web.Service;
using Store.Web.Service.IService;
using System.IdentityModel.Tokens.Jwt;

namespace Store.Web.Controllers
{
    public class CartController : Controller
    {
        private ICartService _cartService;
        private IOrderService _orderService;

        public CartController(ICartService cartService, IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartBasedOnLoggedInUser());
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartBasedOnLoggedInUser());
        }

        [HttpPost]
        [ActionName("Checkout")]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {

            CartDto cart = await LoadCartBasedOnLoggedInUser();
            cart.CartHeader.Phone = cartDto.CartHeader.Phone;
            cart.CartHeader.Email = cartDto.CartHeader.Email;
            cart.CartHeader.Name = cartDto.CartHeader.Name;

            var response = await _orderService.CreateOrder(cart);
            OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

            if (response != null && response.IsSuccess)
            {
                //get stripe session and redirect to stripe to place order

            }
            return View();
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            ResponseDto responseDto = await _cartService.RemoveFromCartAsync(cartDetailsId);
            if(responseDto != null && responseDto.IsSuccess)
            {
                TempData["success"] = "Item removed successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            ResponseDto responseDto = await _cartService.ApplyCouponAsync(cartDto);
            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["success"] = "Coupon applied successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {
            CartDto cart = await LoadCartBasedOnLoggedInUser();
            cart.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;
            ResponseDto responseDto = await _cartService.EmailCartAsync(cart);
            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["success"] = "Email will be processed and sent shortly";
                return RedirectToAction(nameof(CartIndex));
            }
            return View(nameof(CartIndex));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartHeader.CouponCode = "";
            ResponseDto responseDto = await _cartService.ApplyCouponAsync(cartDto);
            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["success"] = "Coupon removed successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        private async Task<CartDto> LoadCartBasedOnLoggedInUser()
        {
            string? userId = User.Claims.First(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            ResponseDto response = await _cartService.GetCartByUserId(userId);
            if(response !=null && response.IsSuccess)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
                return cartDto;
            }
            return new CartDto();
        }
    }
}
