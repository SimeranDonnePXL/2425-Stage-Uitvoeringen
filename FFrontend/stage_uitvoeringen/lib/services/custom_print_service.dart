import 'package:flutter/services.dart';
import 'package:stage_uitvoeringen/models/printer.dart';

class CustomPrintService{
  static final printServiceChannel = new MethodChannel("stage_uitvoering/custom_print_service");

  static Future<void> sendPrintersToAndroid(List<Printer> printers) async {
    final List<Map<String, dynamic>> data = printers
        .map((printer) => {
              'id': printer.id.toString(),
              'name': printer.name,
            })
        .toList();

    try {
      await printServiceChannel.invokeMethod('setPrinters', {'printers': data});
    } on PlatformException catch (e) {
      print('Error sending printers to Android: ${e.message}');
    }
  }
}