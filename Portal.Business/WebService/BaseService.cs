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
        private readonly string _endpoint;

        private static readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri(EndPoints.BASE_URL)
        };

        public BaseService(string endPoint)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");

            _endpoint = endPoint;
        }

        public async Task<MessageResponse> Post(T model)
        {
            try
            {
                string json = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(_endpoint, content);
                string respuestaJson = await response.Content.ReadAsStringAsync();

                try
                {
                    return JsonConvert.DeserializeObject<MessageResponse>(respuestaJson)
                           ?? new MessageResponse { ResponseType = ResponseType.Error, Message = "Respuesta vacía" };
                }
                catch (JsonException)
                {
                    return new MessageResponse
                    {
                        ResponseType = ResponseType.Error,
                        Message = $"Respuesta no válida: {respuestaJson}"
                    };
                }

            }
            catch (HttpRequestException e)
            {
                return new MessageResponse()
                {
                    Message = $"{e.Message} {e?.InnerException?.Message}",
                    ResponseType = ResponseType.Error
                };
            }
        }

    }
}
