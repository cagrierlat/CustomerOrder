using CustomerOrder.API.Contracts.Products;
using CustomerOrder.Data.Context;
using CustomerOrder.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Caching.Distributed;

namespace CustomerOrder.API.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IDistributedCache _cache;
        private readonly IDatabase _database;
        private readonly ILogger<ProductsService> _logger;
        private readonly DataContext _dbContext;
        public ProductsService(ILogger<ProductsService> logger, DataContext context, IDistributedCache cache)
        {
            //_redis = redis;
            //_database = _redis.GetDatabase();
            _logger = logger;
            _dbContext = context;
            _cache = cache;
        }
        public async Task<List<Product>> GetAllProductsAsync()
        {
            try
            {
                var cacheKey = "productList";
                var cachedProducts = await _cache.GetStringAsync(cacheKey);

                if (!string.IsNullOrEmpty(cachedProducts))
                {
                    // Önbellekten verileri döndür
                    return JsonConvert.DeserializeObject<List<Product>>(cachedProducts);
                }

                // Veritabanından verileri al
                var products = await _dbContext.Products.ToListAsync();

                // Verileri önbelleğe kaydet
                await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(products),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // 30 dakika geçerlilik
                    });

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAllProductsAsync error: " + ex.Message);
                throw new Exception($"An error occurred while getall products: {ex.Message}");
            }
        }
    }
}
