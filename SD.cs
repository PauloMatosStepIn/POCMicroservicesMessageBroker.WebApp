namespace StepIn.Web
{
  public class SD
  {
    public static string CouponAPIBase { get; set; }
    public static string OrderAPIBase { get; set; }
    public enum APIType
    {
      GET,
      POST,
      PUT,
      DELETE
    }
  }
}
