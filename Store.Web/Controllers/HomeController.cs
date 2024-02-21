using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Store.Web.Models;
using Store.Web.Service.IService;
using System.Diagnostics;

namespace Store.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
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
