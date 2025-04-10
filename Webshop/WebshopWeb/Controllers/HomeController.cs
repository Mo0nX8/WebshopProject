using Microsoft.AspNetCore.Mvc;
using Webshop.EntityFramework.Managers.Product;
using Webshop.EntityFramework.Managers.User;
using Webshop.Services.Interfaces;
using Webshop.Services.Services.ViewModel;

public class HomeController : Controller
{
    private readonly IUserManager userManager;
    private readonly IOrderServices orderServices;
    private readonly IProductServices productServices;
    private IEncryptManager encryptManager;
    private IProductManager productManager;
    /// <summary>
    /// Initializes a new instance of the <see cref="HomeController"/> class.
    /// </summary>
    /// <param name="userManager">The service responsible for managing user-related operations.</param>
    /// <param name="encryptManager">The service responsible for handling encryption operations.</param>
    /// <param name="productManager">The service responsible for managing product-related operations.</param>
    /// <param name="orderServices">The service responsible for order-related services.</param>
    /// <param name="productServices">The service responsible for product-related services.</param>
    public HomeController(IUserManager userManager,IEncryptManager encryptManager,IProductManager productManager, IOrderServices orderServices, IProductServices productServices)
    {
        this.userManager = userManager;
        this.encryptManager = encryptManager;
        this.productManager = productManager;
        this.orderServices = orderServices;
        this.productServices = productServices;
    }
    /// <summary>
    /// Displays the home page with a random selection of products.
    /// </summary>
    /// <returns>The view with the list of products and the total item count.</returns>
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
    /// <summary>
    /// Displays the user's personal data page if authenticated.
    /// </summary>
    /// <returns>The view with personal data, or redirects to login if not authenticated.</returns>
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
    /// <summary>
    /// Handles the password change request.
    /// </summary>
    /// <param name="model">The model containing current and new passwords.</param>
    /// <returns>A view displaying the result of the password change operation.</returns>
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

    /// <summary>
    /// Displays the partial view for changing the password.
    /// </summary>
    /// <returns>The partial view for changing the password.</returns>
    public PartialViewResult _ChangePasswordPartial()
    {
        return PartialView("_ChangePasswordPartial", new PasswordChangeViewModel());
    }
    /// <summary>
    /// Displays the user's past orders.
    /// </summary>
    /// <returns>The view displaying the list of orders.</returns>
    public IActionResult MyOrders()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var orders = orderServices.GetOrders(userId.Value);
        return View(orders);
    }
    /// <summary>
    /// Displays the terms and conditions page (ASZF).
    /// </summary>
    /// <returns>The view displaying the terms and conditions.</returns>
    public IActionResult aszf()
    {
        return View();
    }
}
