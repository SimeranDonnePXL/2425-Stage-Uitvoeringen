using System.Net.WebSockets;
using System.Text;
using System.Xml.Linq;

namespace Backend
{
    public class WebSocketManager
    {
        private readonly List<WebSocket> _sockets = new();

        public async Task Handle(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var socket = await context.WebSockets.AcceptWebSocketAsync();
                _sockets.Add(socket);

                var buffer = new byte[1024 * 4];

                try
                {
                    while (socket.State == WebSocketState.Open)
                    {
                        var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            Console.WriteLine("Client requested close.");
                            break;
                        }

                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        Console.WriteLine($"Received: {message}");
                    }
                }
                catch (WebSocketException ex)
                {
                    Console.WriteLine($"WebSocket exception: {ex.Message}");
                }
                finally
                {
                    _sockets.Remove(socket);

                    if (socket.State == WebSocketState.Open ||
                        socket.State == WebSocketState.CloseReceived ||
                        socket.State == WebSocketState.CloseSent)
                    {
                        try
                        {
                            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                        }
                        catch (WebSocketException ex)
                        {
                            Console.WriteLine($"WebSocket close failed: {ex.Message}");
                        }
                    }

                    Console.WriteLine("WebSocket connection cleaned up.");
                }
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }


        public async Task BroadcastAsync(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(buffer);

            foreach (var socket in _sockets.ToList())
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }

}
