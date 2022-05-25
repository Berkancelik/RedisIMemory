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
            // aşağıdaki option ile birlikte yukarıdaki options a belirttiğimiz zaman kadar tanımlanmaktadır.
            _memoryCache.Set<string>("Times", DateTime.Now.ToString(), options);
            return View();
        }
        public IActionResult Show()
        {
            _memoryCache.TryGetValue("Times", out string TimesCache);
            ViewBag.time = TimesCache;
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
