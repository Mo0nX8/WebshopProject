using Microsoft.AspNetCore.Mvc;
using Webshop.EntityFramework;
using Webshop.EntityFramework.Managers.Order;
using Webshop.EntityFramework.Managers.Product;
using Webshop.EntityFramework.Managers.User;
using Webshop.Services.Interfaces;
using Webshop.Services.Services.ViewModel;

public class HomeController : Controller
{
    private readonly IUserManager userManager;
    private readonly IAuthenticationManager authenticationManager;
    private readonly GlobalDbContext context;
    private readonly IOrderServices orderServices;
    private readonly IProductServices productServices;
    private IEncryptManager encryptManager;
    private IOrderManager orderManager;
    private IProductManager productManager;

    public HomeController(IAuthenticationManager authenticationManager, IUserManager userManager, GlobalDbContext context, IEncryptManager encryptManager, IOrderManager orderManager, IProductManager productManager, IOrderServices orderServices, IProductServices productServices)
    {
        this.authenticationManager = authenticationManager;
        this.userManager = userManager;
        this.context = context;
        this.encryptManager = encryptManager;
        this.orderManager = orderManager;
        this.productManager = productManager;
        this.orderServices = orderServices;
        this.productServices = productServices;
    }

    public IActionResult Index()
    {
        var totalItems = productManager.CountProducts();
        var products = productServices.GetRandomProducts(totalItems);

        var model = new ProductFilterViewModel
        {
            Products = products,
            TotalItems = totalItems
        };
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
                PasswordChanged=false

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
                PasswordChanged=false

            };
            return View("PersonalData", personalDataViewModel);
        }

        user.Password = encryptManager.Hash(model.NewPassword);
        userManager.UpdateUser(user);

        var personalDataViewModelGood = new PersonalDataViewModel
        {
            UserId = userId.Value,
            UserName = user.Username,
            Email = user.EmailAddress,
            PasswordChanged = true
        };
        return View("PersonalData", personalDataViewModelGood);
    }

   
    public PartialViewResult _ChangePasswordPartial()
    {
        return PartialView("_ChangePasswordPartial", new PasswordChangeViewModel());
    }
    public IActionResult MyOrders()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var orders = orderServices.GetOrders(userId.Value);
        return View(orders);
    }
    public IActionResult aszf()
    {
        return View();
    }
}
