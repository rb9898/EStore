using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Store.Web.Models;
using Store.Web.Service.IService;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace Store.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> list = new();
            ResponseDto? responseDto = await _productService.GetAllProductAsync();
            if (responseDto != null && responseDto.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(responseDto.Result.ToString());
            }
            else
            {
                TempData["error"] = responseDto.Message;
            }
            return View(list);
        }

        [Authorize]
        public async Task<IActionResult> Details(int productId)
        {
            ProductDto model = new();
            ResponseDto? responseDto = await _productService.GetProductByIdAsync(productId);
            if (responseDto != null && responseDto.IsSuccess)
            {
                model = JsonConvert.DeserializeObject<ProductDto>(responseDto.Result.ToString());
            }
            else
            {
                TempData["error"] = responseDto.Message;
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDto productDto)
        {
            CartDto cart = new CartDto()
            {
                CartHeader = new CartHeaderDto()
                {
                    UserId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value
                }
            };
            CartDetailsDto cartDetailsDto = new CartDetailsDto()
            {
                ProductId = productDto.ProductId,
                Count = productDto.Count
            };
            List<CartDetailsDto> cartDetailsDtos = new List<CartDetailsDto>() { cartDetailsDto };
            cart.CartDetails = cartDetailsDtos;
            ResponseDto responseDto = await _cartService.UpsertCart(cart);
            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["success"] = "Item added to cart successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = responseDto.Message;
            }
            return View(productDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
