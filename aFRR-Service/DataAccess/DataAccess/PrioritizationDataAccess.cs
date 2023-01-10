using aFRRService.DTOs;
using DataAccessLayer.Interfaces;
using System.Text;
using System.Text.Json;

namespace DataAccessLayer.DataAccess
{
    public class PrioritizationDataAccess : IPrioritizationDataAccess
    {
        private readonly HttpClient _client;

        public PrioritizationDataAccess(HttpClient client)
        {
           _client = client;
        }

        public async Task<SignalDTO> GetAsync(SignalDTO signalDTO)
        {
            var serializedDTO = JsonSerializer.Serialize(signalDTO);
            StringContent content = new StringContent(serializedDTO, Encoding.UTF8, "application/json");

            using var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("/api/prioritizations/assetregulations", UriKind.Relative),
                Content = content
            };

            var jsonResponse = await _client.SendAsync(request);
            if (jsonResponse.IsSuccessStatusCode)
            {   
                var serializedResponse = await jsonResponse.Content.ReadAsStringAsync();
                var deserializedResponse = JsonSerializer.Deserialize<SignalDTO>(serializedResponse);
                return deserializedResponse;
            }
            else
            {
                throw new Exception($"Error retrieving prioritized regulation for assets");
            }                     
        }
    }
}
