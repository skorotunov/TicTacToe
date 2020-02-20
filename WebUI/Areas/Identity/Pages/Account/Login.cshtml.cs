using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using TicTacToe.Infrastructure.Identity;

namespace TicTacToe.WebUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<TicTacToeUser> signInManager;
        private readonly UserManager<TicTacToeUser> userManager;
        private readonly ILogger<RegisterModel> logger;

        public LoginModel(UserManager<TicTacToeUser> userManager, SignInManager<TicTacToeUser> signInManager, ILogger<RegisterModel> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                // attempt to sign in user
                Microsoft.AspNetCore.Identity.SignInResult signInResult = await signInManager.PasswordSignInAsync(Input.NickName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (signInResult.Succeeded)
                {
                    logger.LogInformation($"User {Input.NickName} logged in.");
                    return LocalRedirect(returnUrl);
                }

                // attempt to sign up user
                var user = new TicTacToeUser { UserName = Input.NickName };
                IdentityResult signUpResult = await userManager.CreateAsync(user, Input.Password);
                if (signUpResult.Succeeded)
                {
                    logger.LogInformation($"User {Input.NickName} created.");
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl ?? Url.Content("~/"));
                }

                foreach (IdentityError error in signUpResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public class InputModel
        {
            [Required]
            [StringLength(100, ErrorMessage = "The {0} cannot be longer that {1} characters.")]
            [Display(Name = "NickName")]
            public string NickName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
    }
}
