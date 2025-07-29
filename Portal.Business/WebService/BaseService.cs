using Newtonsoft.Json;
using Portal.Business.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Business.WebService
{
    public class BaseService<T>
    {
        private readonly HttpClient client;
        private readonly string endpoint;
        public BaseService(string endPoint)
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri(EndPoints.BASE_URL)
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            endpoint = endPoint;
        }

        public async Task<MessageResponse> Post(T model)
        {
            string json = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(endpoint, content);
                if (response.IsSuccessStatusCode)
                {
                    string respuestaJson = await response.Content.ReadAsStringAsync();
                    return new MessageResponse()
                    {
                        Message = "OK",
                        ResponseType = ResponseType.OK
                    };
                }
                else
                {
                    return new MessageResponse()
                    {
                        Message = "OK",
                        ResponseType = ResponseType.OK
                    };
                }

            }
            catch (HttpRequestException e)
            {
                return new MessageResponse()
                {
                    Message = "OK",
                    ResponseType = ResponseType.OK
                };
            }

      
        }

    }
}
