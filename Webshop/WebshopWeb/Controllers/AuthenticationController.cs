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
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="userManager">User manager for handling user-related operations.</param>
        /// <param name="encryptManager">Encryption manager for password hashing.</param>
        /// <param name="authenticationManager">Authentication manager for login and logout functionality.</param>
        /// <param name="serviceProvider">Service provider to resolve validation services.</param>
        /// <param name="cartManager">Cart manager for managing user shopping carts.</param>
        /// <param name="context">Database context for interacting with the database.</param>
        /// <param name="emailService">Email service for sending reset emails.</param>
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
        /// <summary>
        /// Displays the login page.
        /// </summary>
        /// <returns>The login view.</returns>
        public IActionResult Login()
        {
            ViewBag.Response = TempData["Code"];
            return View();
        }
        /// <summary>
        /// Displays the registration page.
        /// </summary>
        /// <returns>The registration view.</returns>
        public IActionResult Register()
        {
            ViewBag.Response = TempData["Code"];
            return View();
        }
        /// <summary>
        /// Attempts to log the user in with the provided email and password.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>A redirect to the home page on success, or a redirect back to the login page on failure.</returns>
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
        /// <summary>
        /// Attempts to register a new user with the provided username, email, and password.
        /// </summary>
        /// <param name="username">The user's desired username.</param>
        /// <param name="email">The user's email address.</param>
        /// <param name="password1">The user's password.</param>
        /// <param name="password2">The confirmation of the user's password.</param>
        /// <returns>A redirect to the login page on success, or back to the registration page on failure.</returns>
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
        /// <summary>
        /// Logs the user out and clears the session.
        /// </summary>
        /// <returns>A redirect to the home page.</returns>
        public IActionResult Logout()
        {
            authenticationManager.LogOut();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Initiates Google login authentication.
        /// </summary>
        /// <returns>A challenge to authenticate with Google.</returns>
        [HttpGet("auth/google-login")]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("ExternalResponse", "Authentication", new {provider="Google"});
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        /// <summary>
        /// Initiates Facebook login authentication.
        /// </summary>
        /// <returns>A challenge to authenticate with Facebook.</returns>
        [HttpGet("auth/facebook-login")]
        public IActionResult FacebookLogin()
        {
            var redirectUrl = Url.Action("ExternalResponse", "Authentication", new { provider = "Facebook" });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }
        /// <summary>
        /// Initiates GitHub login authentication.
        /// </summary>
        /// <returns>A challenge to authenticate with GitHub.</returns>
        [HttpGet("auth/github-login")]
        public IActionResult GithubLogin()
        {
            var redirectUrl = Url.Action("ExternalResponse", "Authentication", new { provider = "GitHub" });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GitHubAuthenticationDefaults.AuthenticationScheme);
        }
        /// <summary>
        /// Handles the external authentication response and processes the user's claims.
        /// </summary>
        /// <param name="provider">The provider used for authentication (Google, Facebook, GitHub).</param>
        /// <returns>A redirect to the home page on success, or a redirect to the login page on failure.</returns>
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
        /// <summary>
        /// Displays the password reset request view where the user can submit their email.
        /// </summary>
        /// <returns>The password reset request view.</returns>

        [HttpGet]
        public IActionResult PasswordResetRequest()
        {
            return View();
        }
        /// <summary>
        /// Handles the POST request for password reset by generating a verification code and sending it to the user's email.
        /// </summary>
        /// <param name="email">The email address submitted by the user.</param>
        /// <param name="code">The verification code submitted by the user (if applicable).</param>
        /// <returns>A view to verify the code or an error message if the user is not found.</returns>
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
        /// <summary>
        /// Displays the view to enter the verification code sent to the user's email.
        /// </summary>
        /// <param name="model">The model containing the user's email for verification.</param>
        /// <returns>The view to verify the code.</returns>
        [HttpGet]
        public IActionResult VerifyCode(ForgotPasswordViewModel model)
        {      
            return View(model.Email);
        }
        /// <summary>
        /// Verifies the code entered by the user. If the code is valid, allows password reset.
        /// </summary>
        /// <param name="email">The email address associated with the user.</param>
        /// <param name="code">The verification code submitted by the user.</param>
        /// <returns>A view to reset the password or an error message if the code is invalid or expired.</returns>
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
        /// <summary>
        /// Displays the password reset form where the user can set a new password.
        /// </summary>
        /// <param name="email">The email address associated with the user requesting the password reset.</param>
        /// <returns>The password reset view.</returns>
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
