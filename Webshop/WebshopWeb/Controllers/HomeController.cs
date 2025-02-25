using Microsoft.AspNetCore.Mvc;
using Webshop.EntityFramework.Managers.Interfaces.User;
using Webshop.Services.Services.ViewModel;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework;
using Webshop.Services.Interfaces_For_Services;
using Webshop.EntityFramework.Managers.Interfaces.Order;
using Microsoft.EntityFrameworkCore;
using Webshop.EntityFramework.Managers.Implementations;
using Webshop.EntityFramework.Managers.Interfaces.Product;

public class HomeController : Controller
{
    private readonly IUserManager userManager;
    private readonly IAuthenticationManager authenticationManager;
    private readonly GlobalDbContext context;
    private IEncryptManager encryptManager;
    private IOrderManager orderManager;
    private IProductManager productManager;

    public HomeController(IAuthenticationManager authenticationManager, IUserManager userManager, GlobalDbContext context, IEncryptManager encryptManager, IOrderManager orderManager, IProductManager productManager)
    {
        this.authenticationManager = authenticationManager;
        this.userManager = userManager;
        this.context = context;
        this.encryptManager = encryptManager;
        this.orderManager = orderManager;
        this.productManager = productManager;
    }

    public IActionResult Index()
    {
        var totalItems = productManager.Count();
        Random random = new Random();
        var skipCount=random.Next(0,totalItems);
        var products = productManager.GetProducts()
            .OrderBy(p=>Guid.NewGuid())
            .Where(p=>p.Price!=0)
            .Skip(skipCount)
            .Take(15)
            .ToList();

        var model = new ProductFilterViewModel
        {
            Products = products,
            TotalItems = totalItems
        };
        ViewBag.IsAuthenticated=true;
        return View(model);
    }

    public IActionResult PersonalData()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var authenticated = HttpContext.Session.GetString("IsAuthenticated");
        if (userId == null)
        {
            return RedirectToAction("Login", "Authentication");
        }

        if (authenticated=="False")
        {
            return RedirectToAction("Login", "Authentication");
        }

        var user = userManager.GetUser(userId.Value);
        if (user == null)
        {
            return NotFound();
        }

        var model = new PersonalDataViewModel
        {
            UserId = userId.Value,
            UserName = user.Username,
            Email = user.EmailAddress
        };

        return View("PersonalData", model);
    }

    [HttpPost]
    public IActionResult ChangePassword(PasswordChangeViewModel model)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var user = userManager.GetUser(userId.Value);
        if (model == null || string.IsNullOrEmpty(model.CurrentPassword) || string.IsNullOrEmpty(model.NewPassword))
        {
            ModelState.AddModelError("", "Kérem, adja meg a régi és az új jelszót.");
            var personalDataViewModel = new PersonalDataViewModel
            {
                UserName = user.Username,
                Email=user.EmailAddress,

            };
            return View("PersonalData", personalDataViewModel);
        }
        
        if (userId == null)
        {
            return RedirectToAction("Login", "Authentication");
        }

        
        var HashedCurrentPassword = encryptManager.Hash(model.CurrentPassword);
        if (user == null || user.Password != HashedCurrentPassword)
        {
            ModelState.AddModelError("", "A mostani jelszó nem megfelelõ.");
            var personalDataViewModel = new PersonalDataViewModel
            {
                UserName = user.Username,
                Email = user.EmailAddress,

            };
            return View("PersonalData", personalDataViewModel);
        }

        user.Password = encryptManager.Hash(model.NewPassword);
        userManager.Update(user);

        System.Threading.Thread.Sleep(5000);
        authenticationManager.LogOut();
        return RedirectToAction("Index");
    }

   
    public PartialViewResult _ChangePasswordPartial()
    {
        return PartialView("_ChangePasswordPartial", new PasswordChangeViewModel());
    }
    public IActionResult MyOrders()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var orders=orderManager.GetOrders()
            .Where(x => x.UserId == userId)
            .Include(x => x.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToList();
        if (orders is null)
        {
            orders = new List<Orders>();
        }
        return View(orders);
    }
    public IActionResult aszf()
    {
        return View();
    }
}
