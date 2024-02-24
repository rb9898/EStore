using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Services.ShoppingCartAPI.Data;
using Store.Services.ShoppingCartAPI.Models;
using Store.Services.ShoppingCartAPI.Models.Dto;
using System.Reflection.PortableExecutable;

namespace Store.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private IMapper _mapper;
        private ResponseDto _responseDto;
        private AppDbContext _appDbContext;

        public CartAPIController(IMapper mapper, AppDbContext appDbContext)
        {
            _mapper = mapper;
            _responseDto = new ResponseDto();
            _appDbContext = appDbContext;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _appDbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u=>u.UserId ==cartDto.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _appDbContext.CartHeaders.Add(cartHeader);
                    await _appDbContext.SaveChangesAsync();
                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _appDbContext.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _appDbContext.SaveChangesAsync();
                }
                else 
                { 
                    var productDetailsFromDb = await _appDbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(u=>u.ProductId == cartDto.CartDetails.First().ProductId && 
                    u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if (productDetailsFromDb == null)
                    {
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _appDbContext.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _appDbContext.SaveChangesAsync();
                    }
                    else
                    {
                        cartDto.CartDetails.First().Count += productDetailsFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = productDetailsFromDb.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = productDetailsFromDb.CartDetailsId;
                        _appDbContext.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _appDbContext.SaveChangesAsync();
                    }
                }
                _responseDto.Result = cartDto;
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message.ToString();
                _responseDto.IsSuccess = false;
            }
            return _responseDto;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = _appDbContext.CartDetails.First(u => u.CartDetailsId == cartDetailsId);
                int itemRemaining = _appDbContext.CartDetails.Where(u=> u.CartHeaderId == cartDetails.CartHeaderId).Count();
                _appDbContext.CartDetails.Remove(cartDetails);
                if(itemRemaining == 1)
                {
                    CartHeader cartHeader = _appDbContext.CartHeaders.First(u => u.CartHeaderId==cartDetails.CartHeaderId);
                    _appDbContext.CartHeaders.Remove(cartHeader);
                }
                await _appDbContext.SaveChangesAsync();
                _responseDto.Result = true;
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message.ToString();
                _responseDto.IsSuccess = false;
            }
            return _responseDto;
        }
    }
}
