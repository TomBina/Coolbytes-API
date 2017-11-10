﻿using System;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using CoolBytes.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CoolBytes.WebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> GetUser()
        {
            var identifier = _httpContextAccessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identifier))
                throw new ArgumentException(nameof(identifier));

            var user = await _appDbContext.Users.SingleOrDefaultAsync(u => u.Identifier == identifier);
            if (user != null)
                return user;

            user = new User(identifier);
            await _appDbContext.SaveChangesAsync();

            return user;
        }
    }
}