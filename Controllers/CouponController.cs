using StepIn.Web.Models;
using StepIn.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace StepIn.Web.Controllers
{
  [Authorize]
  public class CouponController : Controller
  {
    private readonly ICouponService _couponService;

    private readonly ILogger<CouponController> _logger;
    public CouponController(ICouponService couponService, ILogger<CouponController> logger)
    {
      _couponService = couponService;
      _logger = logger;
    }

    public IActionResult CouponIndex()
    {
      List<string> CouponCodes = new List<string>()
        {"01DISCOUNT","02DISCOUNT","03DISCOUNT","04DISCOUNT","05DISCOUNT","06DISCOUNT"};
      ViewBag.message = CouponCodes;

      _logger.LogInformation(CouponCodes.ToString());

      CouponDto coupon = new CouponDto()
      {

      };

      return View(coupon);
    }


    [HttpPost]
    [ActionName("AskCoupon")]
    public async Task<IActionResult> AskCoupon(string CouponCode, string button)
    {

      List<string> CouponCodes = new List<string>()
        {"01DISCOUNT","02DISCOUNT","03DISCOUNT","04DISCOUNT","05DISCOUNT","06DISCOUNT"};
      ViewBag.message = CouponCodes;

      if (button == "ask")
      {
        CouponDto coupon = new CouponDto()
        {
          CouponCode = CouponCode,
          CouponDate = DateTime.Now
        };

        _logger.LogInformation(coupon.CouponCode);
        _logger.LogInformation(coupon.CouponDate.ToString("yyyy/MM/dd hh:mm:ss tt"));

        var userId = User.Claims.Where(u => u.Type == "sub").FirstOrDefault().Value;
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var response = await _couponService.AskCoupon<ResponseDto>(coupon, accessToken);

        if (response != null && response.IsSuccess)
        {
          return View("Confirmation", coupon);
        }
        else if (response != null && !response.IsSuccess)
        {
          foreach (var message in response.ErrorMessages)
          {
            ModelState.AddModelError(string.Empty, message);
          }
        }

        ModelState.AddModelError(string.Empty, "It was not possible to process the order!");

        return View(nameof(CouponIndex));

      }

      return RedirectToAction(nameof(Index), "Home");

    }
  }
}
