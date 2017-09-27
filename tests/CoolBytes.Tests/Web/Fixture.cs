using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.AutoMapper;
using CoolBytes.WebAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CoolBytes.Tests.Web
{
    public class Fixture
    {
        private static readonly Random Random = new Random();

        public Fixture()
        {
            Mapper.Initialize(config => config.AddProfile(new DefaultProfile()));

            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("Test" + Random.Next()).Options;
        }

        public AppDbContext GetNewContext() => new AppDbContext(_options);
        private readonly DbContextOptions<AppDbContext> _options;
    }
}
