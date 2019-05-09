using AutoMapper;
using CoolBytes.Data;
using CoolBytes.Services.Caching;

namespace CoolBytes.WebAPI.Handlers
{
    public class HandlerContext<T>
    {
        public IMapper Mapper { get; }
        public AppDbContext DbContext { get; }
        public ICacheService Cache { get; }

        public HandlerContext(IMapper mapper, AppDbContext dbContext, ICacheService cache)
        {
            Mapper = mapper;
            DbContext = dbContext;
            Cache = cache;
        }

        public T Map(object obj)
            => Mapper.Map<T>(obj);
    }
}