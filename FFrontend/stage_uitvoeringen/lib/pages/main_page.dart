import 'package:flutter/material.dart';
import 'package:stage_uitvoeringen/services/custom_print_service.dart';
import '../models/printer.dart';
import '../services/print_service.dart';
import '../services/websocket_service.dart';

class MainPage extends StatefulWidget {
  const MainPage({super.key});

  @override
  State<MainPage> createState() => _MainPageState();
}

class _MainPageState extends State<MainPage> {
  final PrintService _printerService = PrintService();
  final WebSocketService _ws = WebSocketService();

  List<Printer> _printers = [];
  final List<String> _statusFeed = [];
  int? _selectedPrinterIndex;
  String _loadTime = '';

  @override
  void initState() {
    super.initState();
    _loadPrinters();
    _connectWebSocket();
  }

  Future<void> _loadPrinters() async {
    final stopwatch = Stopwatch()..start();

    _printers = await _printerService.getPrintersAsync();

    await CustomPrintService.sendPrintersToAndroid(_printers);


    stopwatch.stop();
    setState(() {
      _loadTime = 'LoadPrinters took ${stopwatch.elapsedMilliseconds} ms';
    });
  }

  void _connectWebSocket() {
    _ws.onMessage = (msg) {
      setState(() {
        _statusFeed.insert(0, msg);
      });
    };
    _ws.connectAsync();
  }

  Future<void> _onPrintClicked() async {
    if (_selectedPrinterIndex == null) {
      _showAlert('Error', 'Please select a printer.');
      return;
    }

    final selectedPrinter = _printers[_selectedPrinterIndex!];
    await _printerService.submitPrintJobAsync(selectedPrinter.id);
  }

  Future<void> _showAlert(String title, String message) async {
    if (context.mounted) {
      showDialog(
        context: context,
        builder: (_) => AlertDialog(
          title: Text(title),
          content: Text(message),
          actions: [
            TextButton(
              child: const Text('OK'),
              onPressed: () => Navigator.of(context).pop(),
            ),
          ],
        ),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Print Manager')),
      body: Padding(
        padding: const EdgeInsets.all(20),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            DropdownButton<int>(
              hint: const Text('Select a Printer'),
              isExpanded: true,
              value: _selectedPrinterIndex,
              items: _printers.asMap().entries.map((entry) {
                final index = entry.key;
                final printer = entry.value;
                return DropdownMenuItem<int>(
                  value: index,
                  child: Text(printer.name),
                );
              }).toList(),
              onChanged: (value) {
                setState(() {
                  _selectedPrinterIndex = value;
                });
              },
            ),
            const SizedBox(height: 15),
            ElevatedButton(
              onPressed: _onPrintClicked,
              child: const Text('Send Print Job'),
            ),
            const SizedBox(height: 15),
            const Text(
              'Status Feed:',
              style: TextStyle(fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 10),
            Expanded(
              child: ListView.builder(
                itemCount: _statusFeed.length,
                itemBuilder: (context, index) {
                  return Text(_statusFeed[index]);
                },
              ),
            ),
            const SizedBox(height: 10),
            Text(_loadTime),
          ],
        ),
      ),
    );
  }
}
