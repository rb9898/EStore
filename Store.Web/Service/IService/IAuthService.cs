﻿using Store.Web.Models;

namespace Store.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto assignRoleRequestDto);
    }
}