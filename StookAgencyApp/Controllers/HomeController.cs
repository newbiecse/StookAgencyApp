using System;
using System.Linq;
using System.Web.Mvc;

namespace StookAgencyApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public ActionResult Index()
        {
            var customers = _appDbContext.Customers.ToList();
            return View(customers);
        }
    }
}