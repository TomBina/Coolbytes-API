using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Data;
using CoolBytes.WebAPI.AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.Tests.Web
{
    public class Fixture : IDisposable
    {
        private static readonly Random Random = new Random();

        public Fixture()
        {
            var dbName = "Test" + Random.Next();

            Context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(dbName).Options);
            Mapper.Initialize(config => config.AddProfile(new DefaultProfile()));
        }

        public AppDbContext Context { get; }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
