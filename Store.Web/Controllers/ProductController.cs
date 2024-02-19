using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Store.Web.Models;
using Store.Web.Service.IService;

namespace Store.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto> list = new();
            ResponseDto? responseDto = await _productService.GetAllProductAsync();
            if(responseDto != null && responseDto.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(responseDto.Result.ToString());
            }
            else
            {
                TempData["error"] = responseDto.Message;
            }
            return View(list);
        }

		public async Task<IActionResult> ProductCreate()
		{
			return View();
		}

		[HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto productDto)
        {
            if(ModelState.IsValid)
            {
                ResponseDto? response = await _productService.CreateProductAsync(productDto);
                if(response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response.Message;
                }
            }
            return View(productDto);
        }

        public async Task<IActionResult> ProductDelete(int productId)
        {
            ResponseDto? response = await _productService.GetProductByIdAsync(productId);
            if(response != null && response.IsSuccess)
            {
                ProductDto model = JsonConvert.DeserializeObject<ProductDto>(response.Result.ToString());
                return View(model);
            }
            else
            {
                TempData["error"] = response.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto model)
        {
            ResponseDto? response = await _productService.DeleteProductAsync(model.ProductId);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response.Message;
            }
            return View(model);
        }

        public async Task<IActionResult> ProductUpdate(int productId)
        {
            ResponseDto? response = await _productService.GetProductByIdAsync(productId);
            if (response != null && response.IsSuccess)
            {
                ProductDto model = JsonConvert.DeserializeObject<ProductDto>(response.Result.ToString());
                return View(model);
            }
            else
            {
                TempData["error"] = response.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductUpdate(ProductDto model)
        {
            ResponseDto? response = await _productService.UpdateProductAsync(model);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product updated successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response.Message;
            }
            return View(model);
        }
    }
}
