﻿using Destiny.Core.Flow.Dependency;
using Destiny.Core.Flow.Dtos.Identitys;
using Destiny.Core.Flow.Enums;
using Destiny.Core.Flow.Events.EventBus;
using Destiny.Core.Flow.Extensions;
using Destiny.Core.Flow.IServices.Identity;
using Destiny.Core.Flow.IServices.UserRoles;
using Destiny.Core.Flow.Model.Entities.Identity;
using Destiny.Core.Flow.Security.Jwt;
using Destiny.Core.Flow.Services.Identity.Events;
using Destiny.Core.Flow.Ui;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Destiny.Core.Flow.Services.Identity
{
 
    public class IdentityServices : IIdentityServices
    {
        private readonly SignInManager<User> _signInManager = null;
        private readonly UserManager<User> _userManager = null;
        private readonly IJwtBearerService _jwtBearerService = null;
        private readonly IEventBus _bus;
        public IdentityServices(SignInManager<User> signInManager, UserManager<User> userManager, IJwtBearerService jwtBearerService, IEventBus bus)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtBearerService = jwtBearerService;
            _bus = bus;
        }

        public async Task<(OperationResponse item, Claim[] cliams)> ChangePassword(ChangePassDto dto)
        {
            dto.NotNull(nameof(dto));
            var user = await _userManager.FindByNameAsync(dto.UserName);

            if (user == null)
            {
                return (new OperationResponse("此用户不存在!!", OperationResponseType.Error), new Claim[] { });
            }
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, dto.OldPassword, true);
            if (!signInResult.Succeeded)
            {
                return (OperationResponse.Error("密码不正确!!"), new Claim[] { });
            }

           var result =   await _userManager.ChangePasswordAsync(user, dto.OldPassword,dto.NewPassword);

            if (!result.Succeeded)
            {
                return (result.ToOperationResponse(), new Claim[] { });
            }

            var jwtToken = _jwtBearerService.CreateToken(user.Id, user.UserName);
         
            return (new OperationResponse("修改密码成功!!", new
            {
                AccessToken = jwtToken.AccessToken,
                NickName = user.NickName,
                UserId = user.Id.ToString(),
                AccessExpires = jwtToken.AccessExpires
            }, OperationResponseType.Success), jwtToken.claims);
        }

        public async Task<(OperationResponse item, Claim[] cliams)> Login(LoginDto loginDto)
        {
            loginDto.NotNull(nameof(loginDto));
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
            {
                return (new OperationResponse("此用户不存在!!", OperationResponseType.Error), new Claim[] { });
            }
            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result)
            {
                return (new OperationResponse("密码不正确!!", OperationResponseType.Error), new Claim[] { });
            }

            var jwtToken = _jwtBearerService.CreateToken(user.Id, user.UserName);
            await  _bus.PublishAsync(new IdentityEvent() { UserName= loginDto.UserName});
            return (new OperationResponse("登录成功", new
            {
                AccessToken = jwtToken.AccessToken,
                NickName = user.NickName,
                UserId = user.Id.ToString(),
                AccessExpires = jwtToken.AccessExpires
            }, OperationResponseType.Success), jwtToken.claims);
        }
    }
}
