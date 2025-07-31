using Newtonsoft.Json;
using Portal.Business.Models.DataTables;
using Portal.Business.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Business.WebService
{
    public class ApiBaseService<TRequest>
    {
        private readonly string _endpoint;

        private static readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri(ServiceParameters.BASE_URL)
        };

        public ApiBaseService(string endPoint)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");

            _endpoint = endPoint;
        }

        public async Task<RootResult<TResponse>> List<TResponse>(TRequest model)
        {
            try
            {
                string json = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync($"{_endpoint}/list", content);
                string respuestaJson = await response.Content.ReadAsStringAsync();


                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Error API: {response.StatusCode}");

                var responseJson = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<RootResult<TResponse>>(responseJson);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener datos de la API: " + ex.Message, ex);
            }
        }

        public async Task<MessageResponse<TResponse>> Get<TResponse>(int id)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"{_endpoint}/{id}");
                string respuestaJson = await response.Content.ReadAsStringAsync();

                try
                {
                    return JsonConvert.DeserializeObject<MessageResponse<TResponse>>(respuestaJson)
                           ?? new MessageResponse<TResponse> { ResponseType = ResponseType.Error, Message = "Respuesta vacía" };
                }
                catch (JsonException)
                {
                    return new MessageResponse<TResponse>
                    {
                        ResponseType = ResponseType.Error,
                        Message = $"Respuesta no válida: {respuestaJson}"
                    };
                }

            }
            catch (HttpRequestException e)
            {
                return new MessageResponse<TResponse>()
                {
                    Message = $"{e.Message} {e?.InnerException?.Message}",
                    ResponseType = ResponseType.Error
                };
            }
        }

        public async Task<MessageResponse> Post(TRequest model)
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

        public async Task<MessageResponse> Put(int id, TRequest model)
        {
            try
            {
                string json = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PutAsync($"{_endpoint}/{id}", content);
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

        public async Task<MessageResponse> Delete(int id)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync($"{_endpoint}/{id}");
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

        public async Task<RootResult<TResponse>> List<TResponse>(string catalogUrl)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"{_endpoint}/{catalogUrl}");
                string respuestaJson = await response.Content.ReadAsStringAsync();


                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Error API: {response.StatusCode}");

                var responseJson = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<RootResult<TResponse>>(responseJson);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener datos de la API: " + ex.Message, ex);
            }
        }

        public async Task<MessageResponse<TResponse>> Login<TResponse>(TRequest model)
        {
            try
            {
                string json = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync($"{_endpoint}/login", content);
                string respuestaJson = await response.Content.ReadAsStringAsync();

                try
                {
                    return JsonConvert.DeserializeObject<MessageResponse<TResponse>>(respuestaJson)
                           ?? new MessageResponse<TResponse> { ResponseType = ResponseType.Error, Message = "Respuesta vacía" };
                }
                catch (JsonException)
                {
                    return new MessageResponse<TResponse>
                    {
                        ResponseType = ResponseType.Error,
                        Message = $"Respuesta no válida: {respuestaJson}"
                    };
                }

            }
            catch (HttpRequestException e)
            {
                return new MessageResponse<TResponse>()
                {
                    Message = $"{e.Message} {e?.InnerException?.Message}",
                    ResponseType = ResponseType.Error
                };
            }
        }
    }
}
