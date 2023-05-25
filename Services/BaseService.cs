﻿using StepIn.Web.Models;
using StepIn.Web.Services.IServices;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace StepIn.Web.Services
{
  public class BaseService : IBaseService
  {
    public ResponseDto responseModel { get; set; }
    public IHttpClientFactory httpClient { get; set; }

    public BaseService(IHttpClientFactory httpClient)
    {

      this.httpClient = httpClient;
      this.responseModel = new ResponseDto();
    }
    public async Task<T> SendAsync<T>(ApiRequest apiRequest)
    {
      try
      {
        var client = httpClient.CreateClient("StepInAPI");
        HttpRequestMessage message = new HttpRequestMessage();
        message.Headers.Add("Accept", "application/json");
        message.RequestUri = new Uri(apiRequest.Url);
        client.DefaultRequestHeaders.Clear();
        if (apiRequest.Data != null)
        {
          message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
        }

        if (!string.IsNullOrEmpty(apiRequest.AccessToken))
        {
          client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);
        }

        HttpResponseMessage apiResponse = null;
        switch (apiRequest.APIType)
        {
          case SD.APIType.POST:
            message.Method = HttpMethod.Post;
            break;
          case SD.APIType.PUT:
            message.Method = HttpMethod.Put;
            break;
          case SD.APIType.DELETE:
            message.Method = HttpMethod.Delete;
            break;
          default:
            message.Method = HttpMethod.Get;
            break;
        }
        apiResponse = await client.SendAsync(message);

        var apiContent = await apiResponse.Content.ReadAsStringAsync();
        var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);
        return apiResponseDto;

      }
      catch (Exception ex)
      {
        var dto = new ResponseDto
        {
          DisplayMessage = "Error",
          ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
          IsSuccess = false
        };
        var res = JsonConvert.SerializeObject(dto);
        var apiResponseDto = JsonConvert.DeserializeObject<T>(res);
        return apiResponseDto;
      }
    }

    public void Dispose()
    {
      GC.SuppressFinalize(true);
    }
  }
}
