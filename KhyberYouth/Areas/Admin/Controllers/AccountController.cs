using KhyberYouth.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace KhyberYouth.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<AccountController> loger;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<ApplicationUser> userManager,
                                  SignInManager<ApplicationUser> signInManager,
                                   ILogger<AccountController> loger,
                                   IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.loger = loger;
            _emailSender = emailSender;
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var confirmationLink = Url.Action("ConfirmEmail", "Account",
                    //                                   new { userId = user.Id, token }, Request.Scheme);

                    //await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                    //                                   $"Please confirm your email by clicking <a href='{confirmationLink}'>here</a>");

                    if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUser", "Administration");
                    }

                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
       
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.UserName);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // If ReturnUrl exists, redirect to it; otherwise, go to Admin Dashboard
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    // Generate token and send a password reset email (skipping token generation for now)
                    return RedirectToAction("ConfirmEmail", "Account", new { email = model.Email });
                }

                ModelState.AddModelError("", "No account found with this email.");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Invalid email.");
                return View();
            }

            return RedirectToAction("ResetPassword", "Account", new { email });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Invalid reset password request.");
                return View();
            }

            var model = new ResetPasswordViewModel { Email = email };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid reset request. Email not found.");
                    return View(model);
                }

                var removePasswordResult = await userManager.RemovePasswordAsync(user);
                if (!removePasswordResult.Succeeded)
                {
                    foreach (var error in removePasswordResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

                var addPasswordResult = await userManager.AddPasswordAsync(user, model.Password);
                if (addPasswordResult.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }

                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid data. Please try again.");
            }

            return View(model);
        }



        [AllowAnonymous]
        public IActionResult AccessDenied(string returnUrl)
        {
            return View("AccessDenied");
        }
    }
}
