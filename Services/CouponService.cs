using StepIn.Web.Models;
using StepIn.Web.Services.IServices;

namespace StepIn.Web.Services
{
  public class CouponService : BaseService, ICouponService
  {
    public readonly IHttpClientFactory _clientFactory;

    public CouponService(IHttpClientFactory clientFactory) : base(clientFactory)
    {
      _clientFactory = clientFactory;
    }

    public async Task<T> AskCoupon<T>(CouponDto coupon, string token = null)
    {
      return await this.SendAsync<T>(new ApiRequest()
      {
        APIType = SD.APIType.POST,
        Data = coupon,
        Url = SD.CouponAPIBase + "api/coupon/",
        AccessToken = token
      }
      );
    }

    public async Task<T> GetAllCouponsAsync<T>(string token = null)
    {
      return await this.SendAsync<T>(new ApiRequest()
      {
        APIType = SD.APIType.GET,
        Url = SD.OrderAPIBase + "api/coupon/",
        AccessToken = token
      }
      );
    }
  }
}
