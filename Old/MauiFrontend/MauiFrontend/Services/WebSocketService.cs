using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace MauiFrontend.Services
{
    public class WebSocketService
    {
        private ClientWebSocket _socket = new();
        public event Action<string> OnMessage;

        public async Task ConnectAsync()
        {
            try
            {
                await _socket.ConnectAsync(new Uri("ws://10.0.2.2:5128/ws"), CancellationToken.None);
                _ = ReceiveLoop();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebSocket error: {ex.Message}");
            }
        }

        private async Task ReceiveLoop()
        {
            try
            {
                var buffer = new byte[1024 * 4];

                while (_socket.State == WebSocketState.Open)
                {
                    var result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        OnMessage?.Invoke(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Cause of error: " + ex.Message);
            }
        }
    }
}
