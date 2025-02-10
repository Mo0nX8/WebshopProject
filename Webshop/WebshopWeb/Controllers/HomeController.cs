using Microsoft.AspNetCore.Mvc;
using Webshop.EntityFramework.Managers.Interfaces.User;
using Webshop.Services.Services.ViewModel;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework;
using Webshop.Services.Interfaces_For_Services;

public class HomeController : Controller
{
    private readonly IUserManager userManager;
    private readonly IAuthenticationManager authenticationManager;
    private readonly GlobalDbContext context;
    private IEncryptManager encryptManager;

    public HomeController(IAuthenticationManager authenticationManager, IUserManager userManager, GlobalDbContext context, IEncryptManager encryptManager)
    {
        this.authenticationManager = authenticationManager;
        this.userManager = userManager;
        this.context = context;
        this.encryptManager = encryptManager;
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
            ModelState.AddModelError("", "K�rem, adja meg a r�gi �s az �j jelsz�t.");
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
            ModelState.AddModelError("", "A mostani jelsz� nem megfelel�.");
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
}
