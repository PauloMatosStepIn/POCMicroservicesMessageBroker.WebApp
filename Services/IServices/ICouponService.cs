using StepIn.Web.Models;

namespace StepIn.Web.Services.IServices
{
  public interface ICouponService
  {
    Task<T> AskCoupon<T>(CouponDto coupon, string token = null);
    Task<T> GetAllCouponsAsync<T>(string token = null);
  }
}
