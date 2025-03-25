using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Webshop.EntityFramework;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Carts;
using Webshop.EntityFramework.Managers.User;
using Webshop.Services.Interfaces;
using Webshop.Services.Services.Validators;
using Microsoft.AspNetCore.Authentication.Google;
using Webshop.Services.Services.ViewModel;
using Microsoft.AspNetCore.Authentication.Facebook;
using AspNet.Security.OAuth.GitHub;

namespace WebshopWeb.Controllers
{
    public class AuthenticationController : Controller
    {
        private GlobalDbContext _context;
        private ICartManager cartManager;
        private IUserManager userManager;
        private IEncryptManager encryptManager;
        private IAuthenticationManager authenticationManager;
        private readonly EmailValidator emailValidator;
        private readonly UsernameValidator usernameValidator;
        private readonly PasswordValidator passwordValidator;
        private IEmailService emailService;
        public AuthenticationController(IUserManager userManager, IEncryptManager encryptManager, IAuthenticationManager authenticationManager, IServiceProvider serviceProvider, ICartManager cartManager, GlobalDbContext context, IEmailService emailService)
        {
            this.userManager = userManager;
            this.encryptManager = encryptManager;
            this.authenticationManager = authenticationManager;
            this.emailValidator = serviceProvider.GetService<EmailValidator>();
            this.usernameValidator = serviceProvider.GetService<UsernameValidator>();
            this.passwordValidator = serviceProvider.GetService<PasswordValidator>();
            this.cartManager = cartManager;
            _context = context;
            this.emailService = emailService;
        }

        public IActionResult Login()
        {
            ViewBag.Response = TempData["Code"];
            return View();
        }
        public IActionResult Register()
        {
            ViewBag.Response = TempData["Code"];
            return View();
        }
        [HttpPost]
        public IActionResult TryLog(string email, string password)
        {
            if (authenticationManager.TryLogin(email, password))
            {
                var user = userManager.GetUsers()
                    .Where(x => x.EmailAddress == email)
                    .Include(u => u.Cart)
                    .FirstOrDefault();
                HttpContext.Session.SetInt32("UserId", user.Id);
                var cart = cartManager.GetCart(user.Cart.Id);
                HttpContext.Session.SetInt32("CartId", cart.Id);
                HttpContext.Session.SetString("IsAuthenticated", "True");
                Console.WriteLine(cart.Id);
                return RedirectToAction("Index", "Home");
            }
            TempData["Code"] = "Az email cím és jelszó páros nem egyezik!";
            return RedirectToAction("Login", "Authentication");
        }
        public IActionResult TryRegister(string username, string email, string password1, string password2)
        {

            string emailResponseCode = emailValidator.IsAvailable(email);
            string usernameResponseCode = usernameValidator.IsAvailable(username);
            string passwordResponseCode = "";
            if (password1 == password2)
            {
                passwordResponseCode = passwordValidator.IsAvailable(password1);
            }
            else
            {
                passwordResponseCode = "A két jelszó nem egyezik!";
                ModelState.AddModelError("Password", passwordResponseCode);
            }
            if (emailResponseCode != "200")
            {
                ModelState.AddModelError("EmailAddress", emailResponseCode);
            }
            if (usernameResponseCode != "200")
            {
                ModelState.AddModelError("Username", usernameResponseCode);
            }
            if (ModelState.ErrorCount > 1)
            {
                return View("Register");
            }

            UserData user = new UserData()
            {
                Username = username,
                EmailAddress = email,
                Password = encryptManager.Hash(password1)
            };

            userManager.AddUser(user);
            ShoppingCart cart = new ShoppingCart()
            {
                UserId = user.Id,
                CartItems = new List<CartItem>()
            };
            cartManager.AddCart(cart);

            return RedirectToAction("Login");

        }
        public IActionResult Logout()
        {
            authenticationManager.LogOut();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet("auth/google-login")]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("ExternalResponse", "Authentication", new {provider="Google"});
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        [HttpGet("auth/facebook-login")]
        public IActionResult FacebookLogin()
        {
            var redirectUrl = Url.Action("ExternalResponse", "Authentication", new { provider = "Facebook" });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }
        [HttpGet("auth/github-login")]
        public IActionResult GithubLogin()
        {
            var redirectUrl = Url.Action("ExternalResponse", "Authentication", new { provider = "GitHub" });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GitHubAuthenticationDefaults.AuthenticationScheme);
        }
        [HttpGet("auth/external-response")]
        public async Task<IActionResult> ExternalResponse(string provider)
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
            {
                TempData["Code"] = $"{provider} authentication failed. Please try again.";
                return RedirectToAction("Login");
            }

            var claims = authenticateResult.Principal.Identities.FirstOrDefault()?.Claims;
            var providerId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "User";
            name = name.Replace(" ", ".");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(providerId))
            {
                TempData["Code"] = $"{provider} authentication failed. Please try again.";
                return RedirectToAction("Login");
            }

            Console.WriteLine($"{provider} Authenticated Email: " + email);

            var user = _context.Users.Include(u=>u.Cart)
                .FirstOrDefault(u => u.EmailAddress == email);

            if (user == null)
            {
                user = new UserData
                {
                    Username = name,
                    EmailAddress = email,
                    Password = null,
                    Cart=null
                };

                if (provider == "Google")
                {
                    user.GoogleId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                }
                else if (provider == "Facebook")
                {
                    user.FacebookId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                }
                else if (provider == "GitHub")
                {
                    user.GitHubId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                }

                userManager.AddUser(user);

                ShoppingCart cart = new ShoppingCart
                {
                    UserId = user.Id,
                    CartItems = new List<CartItem>()
                };

                user.Cart = cart;

                cartManager.AddCart(cart);
                userManager.UpdateUser(user);
            }
            else
            {
                if (provider == "Google" && string.IsNullOrEmpty(user.GoogleId))
                    user.GoogleId = providerId;
                else if (provider == "Facebook" && string.IsNullOrEmpty(user.FacebookId))
                    user.FacebookId = providerId;
                else if (provider == "GitHub" && string.IsNullOrEmpty(user.GitHubId))
                    user.GitHubId = providerId;

                userManager.UpdateUser(user);
            }
            var userCart = user.Cart ?? new ShoppingCart { UserId = user.Id, CartItems = new List<CartItem>() };
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetInt32("CartId", userCart.Id);
            HttpContext.Session.SetString("IsAuthenticated", "True");

            return RedirectToAction("Index", "Home");
        }




        [HttpGet]
        public IActionResult PasswordResetRequest()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordResetRequest(string email,string code)
        {
            if(!string.IsNullOrEmpty(email))
            {
                var user=userManager.GetUsers()
                    .FirstOrDefault(x=>x.EmailAddress == email);
                if(user!=null)
                {
                    var verificationCode=new Random().Next(100000,999999).ToString();
                    user.Code = verificationCode;
                    user.ExpirationDate = DateTime.Now.AddMinutes(5);
                    await _context.SaveChangesAsync();

                    await emailService.SendResetEmailAsync(email,"Jelszó visszaállítása",$"Az ellenőrzőkódod:{verificationCode} ");
                    TempData["Info"] = "Elküldtünk egy ellenőrzőkódot a megadott email címre.";
                    return View("VerifyCode", new ForgotPasswordViewModel {Email=email });
                }
            }
            TempData["Error"] = "Nincs ezzel az email címmel regisztrált felhasználó.";
            return View();
        }
        [HttpGet]
        public IActionResult VerifyCode(ForgotPasswordViewModel model)
        {      
            return View(model.Email);
        }
        [HttpPost]
        public async Task<IActionResult> VerifyCode(string email, string code)
        {
            var user=userManager.GetUsers()
                .FirstOrDefault (x=>x.EmailAddress == email);
            if(user!=null)
            {
                if(user.Code == code && user.ExpirationDate> DateTime.Now)
                {
                    TempData["Email"] = email; 
                    return View("ResetPassword", new ForgotPasswordViewModel { Email=email});
                }
                TempData["Error"] = "Érvénytelen vagy lejárt kód. Próbáld újra";
            }
            TempData["Error"] = "Nincs ezzel az email címmel regisztrált felhasználó.";
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("PasswordResetRequest");
            }

            return View(new ForgotPasswordViewModel { Email = email });
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.EmailAddress == model.Email);
                if (user != null)
                {
                   
                    user.Password = encryptManager.Hash(model.NewPassword); 

                   
                    user.Code = null;
                    user.ExpirationDate = null;

                   
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Jelszó sikeresen megváltoztatva!";
                    return RedirectToAction("Login");
                }

                TempData["Error"] = "Nem sikerült visszaállítani a jelszót. Próbáld újra";
            }

            return View(model);
        }

    }
}
