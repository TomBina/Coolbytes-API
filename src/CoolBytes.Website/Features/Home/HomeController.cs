using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoolBytes.Website.Features.Home
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
