using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            Product product = new Product { Id = 1, Name = "Kalem", Price = 100 };
            string jsonProduct = JsonConvert.SerializeObject(product);
            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);
            _distributedCache.Set("product:1",byteProduct);




            //await _distributedCache.SetStringAsync("product:1", jsonProduct, cacheEntryOptions);

            return View();
        }

        public IActionResult Show()
        {
            Byte[] byteProduct = _distributedCache.Get("product:1");
            string jsonProduct = Encoding.UTF8.GetString(byteProduct);

            Product p = JsonConvert.DeserializeObject<Product>(jsonProduct);
            ViewBag.product = p; 
            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("name");
            return View();
        }

    }
}
