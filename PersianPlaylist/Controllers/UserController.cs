using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Domain.Repositories;
using Domain.Entities.Users;
using Domain.Utility;

namespace PersianPlaylist.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserRepo userRepo;

        public UserController(IUnitOfWork _unitOfWork, IUserRepo _userRepo)
        {
            unitOfWork = _unitOfWork;
            userRepo = _userRepo;
        }
        public async Task<IActionResult> SignUp(string mobile,string name)
        {
            
            Random random = new Random();
            int randomCode = random.Next(10000, 99999);//6 Digit number
            if(!Int64.TryParse(mobile.ToEnglishNumber(), out long mobileNumber))
            {
                return BadRequest();
            }
            var user = await userRepo.Get(mobileNumber);
            if(user == null)
            {
                await userRepo.Add(new User
                {
                    ExpireVerificationCode = DateTime.UtcNow.AddSeconds(120),
                    Mobile = mobileNumber,
                    VerificationCode = randomCode,
                    Name = name
                });
            }
            else
            {
                user.VerificationCode = randomCode;
                user.ExpireVerificationCode = DateTime.UtcNow;
                user.Name = name;
                await userRepo.Update(user);
            }

            await unitOfWork.SaveChangesAsync();
            return View();
        }
        public async Task<IActionResult> SignIn(long mobile, int code,bool isRemember)
        {
            if (mobile == 0 || code == 0)
                return BadRequest();
            var result = new ViewModel() { Status = false };
            var user = await userRepo.Get(mobile);
            if (user == null)
            {
                result.Message = "حساب کاربری شما یافت نشد.";
                return Ok(result);
            }

            if (user.VerificationCode == code)
            {
                result.Message = "کد وارد شده معتبر نمی باشد لطفا مجددا تلاش کنید.";
                return Ok(result);
            }
            if (user.ExpireVerificationCode < DateTime.UtcNow)
            {
                result.Message = "کد وارد شده منقضی شده است لطفا مجددا تلاش فرمایید.";
                return Ok(result);
            }

            UserAuthentication(user.Id.ToString(),user.Name ?? "کاربر", isRemember);
            return View();
        }

        private List<Claim> UserAuthentication(string id, string userName, bool isRememberMe = false)
        {
            var claims = new List<Claim>()
            {
                new System.Security.Claims.Claim(ClaimTypes.Name,id),
                new System.Security.Claims.Claim(ClaimTypes.GivenName,userName)
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principle = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties()
            {
                IsPersistent = isRememberMe
            };
            HttpContext.SignInAsync(principle, properties);
            return claims;
        }

        #region Logout
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
