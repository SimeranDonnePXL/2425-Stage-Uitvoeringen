import 'dart:convert';
import 'package:http/http.dart' as http;
import '../models/printer.dart';
import '../models/print_job.dart';

class PrintService {
  final String baseUrl = 'http://10.0.2.2:5128/api/printer';
  final http.Client client = http.Client();

  Future<List<Printer>> getPrintersAsync() async {
    try {
      final response = await client.get(Uri.parse(baseUrl));

      if (response.statusCode == 200) {
        final List<dynamic> data = jsonDecode(response.body);
        final printers = data.map((item) => Printer.fromJson(item)).toList();

        return printers;
      } else {
        print('Error fetching printers: ${response.statusCode}');
        return [];
      }
    } catch (e) {
      print('Exception in getPrintersAsync: $e');
      return [];
    }
  }

  Future<void> submitPrintJobAsync(int printerId) async {
    final job = PrintJob(
      printerId: printerId,
      documentId: printerId, // mimicking the same as in C#
    );

    try {
      final response = await client.post(
        Uri.parse('$baseUrl/print'),
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode(job.toJson()),
      );

      if (response.statusCode >= 200 && response.statusCode < 300) {
        print('Print job submitted successfully');
      } else {
        print('Failed to submit print job: ${response.statusCode}');
      }
    } catch (e) {
      print('Exception in submitPrintJobAsync: $e');
    }
  }
}
