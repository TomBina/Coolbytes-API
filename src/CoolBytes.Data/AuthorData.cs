using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.Data
{
    public class AuthorData : IAuthorData
    {
        private readonly AppDbContext _appDbContext;

        public AuthorData(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Task<bool> AuthorExists(User user) => _appDbContext.Authors.AnyAsync(a => a.UserId == user.Id);
    }
}