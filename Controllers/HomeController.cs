using StepIn.Web.Models;
using StepIn.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StepIn.Web.Controllers
{

  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;

    private readonly ICouponService _couponService;

    public HomeController(ILogger<HomeController> logger, ICouponService couponService)
    {
      _logger = logger;
      _couponService = couponService;
    }

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult specifications()
    {
      return View();
    }

    public async Task<IActionResult> CouponIndex()
    {
      List<CouponDto> coupons = new();
      var response = await _couponService.GetAllCouponsAsync<ResponseDto>("");
      if (response != null && response.IsSuccess)
      {
        coupons = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
      }

      return View(coupons);
    }





    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Authorize]
    public async Task<IActionResult> Login()
    {
      var accessToken = await HttpContext.GetTokenAsync("access_token");
      return RedirectToAction(nameof(Index));
    }

    public IActionResult Logout()
    {
      return SignOut("Cookies", "oidc");
    }
  }
}