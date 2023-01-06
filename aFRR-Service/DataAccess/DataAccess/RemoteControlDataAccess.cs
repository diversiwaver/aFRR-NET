using aFRRService.DTOs;
using DataAccessLayer.Interfaces;
using System.Text;
using System.Text.Json;

namespace DataAccessLayer.DataAccess;

internal class RemoteControlDataAccess : IRemoteControlDataAccess
{
    private HttpClient _client;

    public RemoteControlDataAccess(HttpClient client)
    {
        _client = client;
    }

    public async Task<bool> SendAsync(SignalDTO signalDTO)
    {
        var serializedDTO = JsonSerializer.Serialize(signalDTO);
        StringContent content = new StringContent(serializedDTO, Encoding.UTF8, "application/json");

        using var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/signals/sendsignal", UriKind.Relative),
            Content = content
        };

        var response = await _client.SendAsync(request);
        return response.IsSuccessStatusCode;
    }
}
