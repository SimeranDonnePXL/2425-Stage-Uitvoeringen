import 'package:web_socket_channel/web_socket_channel.dart';
import 'package:web_socket_channel/status.dart' as status;

class WebSocketService {
  late final WebSocketChannel channel;
  late void Function(String message) onMessage;

  Future<void> connectAsync() async {
    try {
      channel = WebSocketChannel.connect(
        Uri.parse('ws://10.0.2.2:5128/ws'),
      );

      // Start listening
      channel.stream.listen(
            (message) {
          onMessage(message);
        },
        onError: (error) {
          print('WebSocket error: $error');
        },
        onDone: () {
          print('WebSocket connection closed.');
        },
      );
    } catch (e) {
      print('WebSocket connection failed: $e');
    }
  }

  void close() {
    channel.sink.close(status.normalClosure);
  }
}
