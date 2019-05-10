using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace MockEndpoint_Spike
{
    public class Client
    {
        private readonly HttpClient _httpClient = null;

        public Client(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Response GetData(string id)
        {
            var response = new Response();
            try
            {
                var temp = _httpClient.GetAsync(new Uri($"http://www.test.com/test/{id}")).GetAwaiter().GetResult();
                response = JsonConvert.DeserializeObject<Response>(temp.Content.ReadAsStringAsync().GetAwaiter().GetResult());

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Messages.Add(ex.Message);
            }

            return response;
        }
    }
}
