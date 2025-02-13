using Microsoft.AspNetCore.Mvc;
using Webshop.EntityFramework.Managers.Interfaces.User;
using Webshop.Services.Services.ViewModel;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework;
using Webshop.Services.Interfaces_For_Services;
using Webshop.EntityFramework.Managers.Interfaces.Order;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
{
    private readonly IUserManager userManager;
    private readonly IAuthenticationManager authenticationManager;
    private readonly GlobalDbContext context;
    private IEncryptManager encryptManager;
    private IOrderManager orderManager;

    public HomeController(IAuthenticationManager authenticationManager, IUserManager userManager, GlobalDbContext context, IEncryptManager encryptManager, IOrderManager orderManager)
    {
        this.authenticationManager = authenticationManager;
        this.userManager = userManager;
        this.context = context;
        this.encryptManager = encryptManager;
        this.orderManager = orderManager;
    }

    public IActionResult Index()
    {
        ViewBag.IsAuthenticated = authenticationManager.IsAuthenticated;
        return View();
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
        if (model == null || string.IsNullOrEmpty(model.CurrentPassword) || string.IsNullOrEmpty(model.NewPassword))
        {
            ModelState.AddModelError("", "Kérem, adja meg a régi és az új jelszót.");
            var personalDataViewModel = new PersonalDataViewModel
            {
                UserName = userManager.GetUser((int)userId).Username,
                Email=userManager.GetUser((int)userId).EmailAddress,

            };
            return View("PersonalData", personalDataViewModel);
        }
        
        if (userId == null)
        {
            return RedirectToAction("Login", "Authentication");
        }

        var user = context.Users.FirstOrDefault(x => x.Id == userId);
        var HashedCurrentPassword = encryptManager.Hash(model.CurrentPassword);
        if (user == null || user.Password != HashedCurrentPassword)
        {
            ModelState.AddModelError("", "A mostani jelszó nem megfelelõ.");
            var personalDataViewModel = new PersonalDataViewModel
            {
                UserName = userManager.GetUser((int)userId).Username,
                Email = userManager.GetUser((int)userId).EmailAddress,

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
        var orders=context.Orders
            .Where(x => x.UserId == userId)
            .Include(x=>x.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToList();
        if(orders is null)
        {
            orders = new List<Orders>();
        }
        return View(orders);
    }
}
