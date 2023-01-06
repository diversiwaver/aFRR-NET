using aFRRService.DTOs;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccessLayer.DataAccess
{
    public class PrioritizationDataAccess : IPrioritizationDataAccess
    {
        private HttpClient _client;

        public PrioritizationDataAccess(HttpClient client)
        {
           _client = client;
        }

        public async Task<SignalDTO> GetAsync(SignalDTO signalDTO)
        {
            var serializedDTO = JsonSerializer.Serialize(signalDTO);
            var jsonResponse = await _client.GetAsync(serializedDTO);
            if (jsonResponse.IsSuccessStatusCode)
            {   
                var response = await jsonResponse.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<SignalDTO>(response);
            }
            else
            {
                throw new Exception($"Error retrieving prioritized regulation for assets");
            }                     
        }
    }
}
