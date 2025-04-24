using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AvansFysio.Models.Authentication;
using Core.DomainServices;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace AvansFysio.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IPatientService _patientService;

        public AuthenticationController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IPatientService patientService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _patientService = patientService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            var model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid) return View();
            var user = await _userManager.FindByEmailAsync(loginModel.Email);

            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);

                if (signInResult.Succeeded)
                {
                    var returnUrl = Request.Headers["Referer"].ToString();

                    if (!string.IsNullOrEmpty(returnUrl) && !returnUrl.Contains("Login"))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(nameof(loginModel.Password), "Verkeerde combinatie");
                }
            }
            else
            {
                ModelState.AddModelError(nameof(loginModel.Email), "Gebruiker kon niet gevonden worden.");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            var model = new RegisterModel();
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            var patient = _patientService.GetPatients().SingleOrDefault(p => p.Email.Equals(registerModel.Email));

            if (patient == null)
            {
                ModelState.AddModelError(nameof(registerModel.Email), "Het email moet overeenkomen met een bestaande patënt.");
            }

            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = registerModel.Email.Split("@")[0],
                    Email = registerModel.Email
                };

                var result = await _userManager.CreateAsync(user, registerModel.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(user, new Claim("Claim.Patient", "Patient"));
                    return RedirectToAction("Index", "Home");
                }

                foreach (IdentityError i in result.Errors)
                {
                    if (i.Code.Equals("PasswordTooShort"))
                    {
                        ModelState.AddModelError(string.Empty, "Wachtwoord moet minimaal 8 karakters lang zijn.");
                    }
                    else if (i.Code.Equals("PasswordRequiresDigit"))
                    {
                        ModelState.AddModelError(string.Empty, "Wachtwoord moet een nummer bevatten.");
                    }
                }
            }

            return View(registerModel);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Authentication");
        }

        public async Task<IActionResult> CreateDefaultUsers()
        {
            var userPhys = new List<IdentityUser>
            {
                new()
                {
                    UserName = "d.m",
                    Email = "d.m@docent.avans.nl"
                },
                new()
                {
                    UserName = "m.frueger",
                    Email = "m.frueger@docent.avans.nl"
                }
            };

            var userInterns = new List<IdentityUser>
            {
                new()
                {
                    UserName = "bam.vheng",
                    Email = "bam.vheng@student.avans.nl"
                },
                new()
                {
                UserName = "r.vrij",
                Email = "r.vrij@student.avans.nl",
                },
                new()
                {
                    UserName = "m.rodriguez",
                    Email = "m.rodrigeuz@student.avans.nl",
                }
            };

            foreach (var phys in userPhys)
            {
                var userPhysExist = await _userManager.FindByEmailAsync(phys.Email);
                if (userPhysExist == null)
                {
                    var resultPhys = await _userManager.CreateAsync(phys, "Physio1@");
                    await _userManager.AddClaimAsync(phys, new Claim("Claim.Physiotherapist", "Physiotherapist"));
                }
            }

            foreach (var intern in userInterns)
            {
                var userInternExist = await _userManager.FindByEmailAsync(intern.Email);
                if (userInternExist == null)
                {
                    var resultIntern = await _userManager.CreateAsync(intern, "Intern1@");
                    await _userManager.AddClaimAsync(intern, new Claim("Claim.Intern", "Intern"));
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
