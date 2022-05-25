using IMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace IMemoryApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public HomeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddSeconds(10);
            //options.SlidingExpiration = TimeSpan.FromSeconds(10);
            options.Priority = CacheItemPriority.High;
            options.RegisterPostEvictionCallback(((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key} -> {value} => sebep{reason}");
            }));
            _memoryCache.Set<string>("Times", DateTime.Now.ToString(), options);
            Product p = new Product { Id = 1, Name = "Kalem", Price = 200 };
            _memoryCache.Set<Product>("product:1", p);
            return View();
        }
        public IActionResult Show()
        {
            _memoryCache.TryGetValue("Times", out string TimesCache);
            _memoryCache.TryGetValue("callback", out string callBack);
            ViewBag.time = TimesCache;
            ViewBag.callBack = callBack;

            ViewBag.p = _memoryCache.Get<Product>("product:1");
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
