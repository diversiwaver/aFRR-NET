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
        private HttpClient client;

        public PrioritizationDataAccess(HttpClient client)
        {
            this.client = client;
        }

        //TODO: test this implementation through the DataAccessFactory creation
        public async Task<SignalDTO> GetAsync(SignalDTO signalDTO)
        {
            var serializedDTO = JsonSerializer.Serialize(signalDTO);
            var jsonResponse = await client.GetAsync(serializedDTO);
            var response = await jsonResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<SignalDTO>(response);           
        }
    }
}
