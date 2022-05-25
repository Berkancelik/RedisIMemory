using IMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
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
            // 1. Tol
            if (String.IsNullOrEmpty(_memoryCache.Get<string>("zaman"))
            {
                _memoryCache.Set<String>("Times", DateTime.Now.ToString());

                //2. Yol 
                // aşağıda hem cache true döner hemde zaman cache sine sahip olan değerin valuesini, timecache değerine atayacaktır
                if (_memoryCache.TryGetValue("Times", out string timescache))
                {
                    _memoryCache.Set<String>("Times", DateTime.Now.ToString());

                }

                timescache






                return View();

            }
        }

        public IActionResult Show()
        {
            //siler
            _memoryCache.Remove("Times");
            // Bu Key'e sahip değeri ara yoksa oluştur
            _memoryCache.GetOrCreate<string>("time", entry =>
            {
                return DateTime.Now.ToString();
            });

            ViewBag.time = _memoryCache.Get<String>("Times");
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
