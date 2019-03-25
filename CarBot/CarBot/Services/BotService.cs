using CarBot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CarBot.Services
{
    public class BotService
    {
        private const string ApiUrl = "http://192.168.100.5:5004";

        private Action<string> onReceiveMessage;
        private HttpClient _httpClient;

        public BotService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(ApiUrl);
        }

        public static string SessionId { get; set; }

        public void AttachOnReceiveMessage(Action<string> onMessageReceived)
        {
            this.onReceiveMessage = onMessageReceived;
        }

        public async Task SendToBot(string text)
        {
            var newMessage = new MessageModel { Message = text };
            var jsonMessage = JsonConvert.SerializeObject(newMessage);
            var content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/chat?userId={SessionId}");
            request.Content = content;
            var response = await _httpClient.SendAsync(request);
            var responseMessage = await response.Content.ReadAsStringAsync();
            this.onReceiveMessage?.Invoke(responseMessage);
        }
    }
}
