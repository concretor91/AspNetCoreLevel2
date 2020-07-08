using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Interfaces;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IValuesService valuesService;
        private readonly ILogger<HomeController> logger;

        public HomeController(IValuesService valuesService, ILogger<HomeController> logger)
        {
            this.valuesService = valuesService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            logger.LogInformation("sdfsdf");
            var values = await valuesService.GetAsync();
            return View(values);
        }

        public IActionResult Throw(string id) => throw new ApplicationException(id ?? "Error!");

        public IActionResult Blog() => View();
        
        public IActionResult BlogSingle() => View();
        
        public IActionResult Cart() => View();
        
        public IActionResult CheckOut() => View();
        
        public IActionResult ContactUs() => View();
        
        public IActionResult Login() => View();
        
        public IActionResult ProductDetails() => View();
        
        public IActionResult Shop() => View();
    }
}